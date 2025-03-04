namespace WebApi.Models;

public class HttpDefaultResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public HttpDefaultResponse(T data, string message = "Dados retornados com sucesso", bool success = true)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}