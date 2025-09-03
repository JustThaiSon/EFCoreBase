using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Helper.Constants.Globals
{
    public enum SocketCodeEnum
    {
        SUCCESS = 1,
        ERR_NOT_IN_LOBBY = -100,
        ERR_NO_ROOM_AVAILABLE = -101,
        ERR_NOT_ENOUGH_PLAYER = -102,
        ERR_IN_ANOTHER_LOBBY = -103,
        ERR_NO_TICKET = -104,
        ERR_PLAYING = -105,
        ERR_NO_PLAYING = -106,
        ERR_FULL_LOBBY = -107,
    }
}
