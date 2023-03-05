namespace LeasesApi.Serialisers
{
    public class SerialiserException : Exception
    {
        public SerialiserException()
        {
        }
        public SerialiserException(string message) : base(message)
        {
        }
        public SerialiserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
