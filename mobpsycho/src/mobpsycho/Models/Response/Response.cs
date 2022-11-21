namespace mobpsycho.Models.Response
{
    public class Response
    {
        // constructores
        public Response(bool success, string mensaje, object data) // Para cuando deba responder los Get con data
        {
            Success = success;
            Message = mensaje;
            Data = data;
        }

        public Response(bool success, string mensaje) // Para cuando deba responder sin data, solo mensaje
        {
            Success = success;
            Message = mensaje;
            Data = null;
        }


        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public object? Data { get; set; }
    }
}
