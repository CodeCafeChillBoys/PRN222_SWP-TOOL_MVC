namespace PRN222_SWP_TOOL_MVC.Service.DTO.Request
{
    public class ReturnData<T>
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }

    }
}
