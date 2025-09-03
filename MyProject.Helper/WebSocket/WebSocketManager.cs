using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using MyProject.Helper.Constants.Globals;
using MyProject.Helper.Utils;
using Newtonsoft.Json;

public class WebSocketManager : IWebSocketManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _userSockets = new();
    private readonly ConcurrentDictionary<string, List<WebSocket>> _groupSockets = new();

    #region Users
    public async Task AddUserSocketAsync(string hub, string userId, WebSocket webSocket)
    {
        await EnsureSingleUserConnectionAsync(hub, userId, webSocket);
    }
    public async Task RemoveUserSocketAsync(string hub, string userId)
    {
        string key = $"{hub}_{userId}";
        _userSockets.TryRemove(key, out _);
    }
    public async Task SendMessageToUserAsync(string hub, string userId,  string message)
    {
        string key = $"{hub}_{userId}";
        if (_userSockets.TryGetValue(key, out WebSocket webSocket) && webSocket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    public async Task SendMessageToAllUserAsync(string hub, string message)
    {
        var hubSockets = _userSockets
        .Where(kvp => {
            var parts = kvp.Key.Split('_');
            return parts.Length == 2 && parts[0] == hub;
        })
        .Select(kvp => kvp.Value)
        .ToList();
        if (hubSockets.Any())
        {
            foreach (var hubSocket in hubSockets)
            {
                if (hubSocket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await hubSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }  
            }
        }
    }

    public async Task RemoveHubSocketAsync (string hub)
    {
        var keysToRemove = _userSockets
        .Where(kvp =>
        {
            var parts = kvp.Key.Split('_');
            return parts.Length == 2 && parts[0] == hub;
        })
        .Select(kvp => kvp.Key)
        .ToList();

        foreach (var key in keysToRemove)
        {
            _userSockets.TryRemove(key, out _);
        }
    }
    public WebSocket? GetSocket(string hub, string userId)
    {
        string key = $"{hub}_{userId}";
        _userSockets.TryGetValue(key, out var socket);
        return socket;
    }

    public async Task ResponseMessage(string hub, string userId, string method, int code)
    {
        var response = new MessageResponse
        {
            Success = code > 0 ? true : false,
            Code = code
        };
        var messageSend = new CommonMessage<MessageResponse>
        {
            MessageId = 1,
            Method = method,
            Data = response
        };
        var json = JsonConvert.SerializeObject(messageSend);
        await SendMessageToNewUserAsync(hub, userId, json);
    }
    public async Task SendMessageToNewUserAsync(string hub, string userId, string message)
    {
        var hubSockets = _userSockets
        .Where(kvp => kvp.Key.StartsWith($"{hub}_{userId}"))
        .Select(kvp => kvp.Value)
        .ToList();
        if (hubSockets.Any())
        {
            foreach (var hubSocket in hubSockets)
            {
                if (hubSocket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await hubSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

    }
    public bool IsUserConnected(string hub, string userId)
    {
        return _userSockets
            .Where(kvp => kvp.Key.StartsWith($"{hub}_{userId}"))
            .Any(kvp => kvp.Value.State == WebSocketState.Open);
    }

    public async Task EnsureSingleUserConnectionAsync(string hub, string userId, WebSocket newWebSocket)
    {
        string key = $"{hub}_{userId}";

        if (_userSockets.TryGetValue(key, out var existingWebSocket))
        {
            if (existingWebSocket.State == WebSocketState.Open ||
                existingWebSocket.State == WebSocketState.CloseReceived ||
                existingWebSocket.State == WebSocketState.CloseSent)
            {
                try
                {
                    await existingWebSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "New connection detected",
                        CancellationToken.None
                    );
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            _userSockets.TryRemove(key, out _);
        }

        _userSockets[key] = newWebSocket;
    }


    #endregion

    #region Group
    public async Task AddToGroupAsync(string groupId, WebSocket webSocket)
    {
        _groupSockets.AddOrUpdate(groupId,
            _ => new List<WebSocket> { webSocket },
            (_, list) =>
            {
                lock (list)
                {
                    list.Add(webSocket);
                }
                return list;
            });
    }
    public async Task SendMessageToGroupAsync(string groupId, string message)
    {
        if (_groupSockets.TryGetValue(groupId, out var webSockets))
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            List<WebSocket> toRemove = new();

            foreach (var webSocket in webSockets)
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    toRemove.Add(webSocket);
                }
            }

            lock (webSockets)
            {
                foreach (var ws in toRemove)
                {
                    webSockets.Remove(ws);
                }
            }
        }
    }
    public async Task DisconnectUserAsync(string hub, string userId)
    {
        string key = $"{hub}_{userId}";
        if (_userSockets.TryRemove(key, out WebSocket webSocket))
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected by server", CancellationToken.None);
            }
        }
    }

    public async Task HandleDisconnectAsync(WebSocket webSocket)
    {
        foreach (var (userId, socket) in _userSockets)
        {
            if (socket == webSocket)
            {
                _userSockets.TryRemove(userId, out _);
                break;
            }
        }

        foreach (var (groupId, webSockets) in _groupSockets)
        {
            if (webSockets.Contains(webSocket))
            {
                lock (webSockets)
                {
                    webSockets.Remove(webSocket);
                }

                if (webSockets.Count == 0)
                {
                    _groupSockets.TryRemove(groupId, out _);
                }
            }
        }
    }
    

    public async Task PingToAllClientAsync (string hub)
    {
        var hubSockets = _userSockets
       .Where(kvp => kvp.Key.StartsWith($"{hub}"))
       .Select(kvp => kvp.Value)
       .ToList();


    }

    #endregion
}
