using Newtonsoft.Json;

namespace careblock_service.ResponseModel;

public class BoolApiResponse
{
    public BoolApiResponse()
    {
    }

    public BoolApiResponse(bool success)
    {
        Success = success;
    }

    public BoolApiResponse(bool success, string message, int code = 0)
    {
        Success = success;
        Message = message;
        Code = code;
    }

    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets message code
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    /// <summary>
    /// Error code
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Code { get; set; }
}