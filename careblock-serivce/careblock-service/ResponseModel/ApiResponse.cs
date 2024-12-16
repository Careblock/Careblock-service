using Newtonsoft.Json;

namespace careblock_service.ResponseModel;

public class ApiResponse<T>
{
    public ApiResponse()
    {
    }

    public ApiResponse(Exception e)
    {
        this.Success = false;
        this.Message = e.Message;
    }

    public ApiResponse(bool success)
    {
        this.Success = success;
    }

    public ApiResponse(T data, bool success)
    {
        this.Data = data;
        this.Success = success;
    }

    public ApiResponse(T data)
    {
        this.Data = data;
        this.Success = true;
    }

    [JsonProperty("success")] 
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets message code
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets data response
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data")]
    public T? Data { get; set; }

    /// <summary>
    /// it's error or success code
    /// </summary>
    public int Code { get; set; }
}