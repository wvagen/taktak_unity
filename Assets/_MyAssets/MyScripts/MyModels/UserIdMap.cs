using System;
using Newtonsoft.Json;
using Postgrest.Models;

public class UserIdMap: BaseModel
{
    public long Id { get; set; }
    public string UserCred { get; set; }
    public string UserId { get; set; }
    public string UserType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = null;

    [JsonIgnore]
    public string CreatedAtFormatted => CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");

    [JsonIgnore]
    public string UpdatedAtFormatted => UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss");

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static UserIdMap FromJson(string json)
    {
        return JsonConvert.DeserializeObject<UserIdMap>(json);
    }
}
