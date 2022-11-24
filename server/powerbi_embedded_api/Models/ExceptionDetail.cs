namespace powerbi_embedded_api.Models
{
    public class ExceptionDetail
    {
        public ExceptionDetail(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}