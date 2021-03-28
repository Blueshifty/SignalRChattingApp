namespace SignalRApp.Business.Utilities.Results
{
    public class Result
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public Result(string message, bool success = true)
        {
            Message = message;
            Success = success;
        }

        public Result() : this("", true)
        {
        }
    }
}