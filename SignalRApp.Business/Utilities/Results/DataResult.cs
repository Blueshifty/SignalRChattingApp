namespace SignalRApp.Business.Utilities.Results
{
    public class DataResult<T> : Result where T : class
    {
        public T Data { get; set; }

        public DataResult(T data, string message, bool success = true) : base(message, success)
        {
            Data = data;
        }


        public DataResult(T data) : base("", true)
        {
            Data = data;
        }

        public DataResult(string message, bool success = true) : base(message, success)
        {
        }
    }
}