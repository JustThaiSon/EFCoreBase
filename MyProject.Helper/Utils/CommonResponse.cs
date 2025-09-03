using Newtonsoft.Json;
namespace MyProject.Helper.Utils
{
    public class CommonResponse<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class CommonMessage<T>
    {
        [JsonProperty("i")]
        public long MessageId { get; set; }
        [JsonProperty("m")]
        public string Method { get; set; }
        [JsonProperty("dt")]
        public T Data { get; set; }
    }

    public class CommonPagination<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int TotalRecord {  get; set; }
    }    
    public class ConnectWalletResponErr<T>
    {
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string[] message { get; set; }
    }
    public class WalletRespon
    {
        public string ReferenceId { get; set; }
        public string Email { get; set; }
    }
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class ApiResponseErr
    {
        public List<string> Message { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }
    }
    public class MessageResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
    }
}