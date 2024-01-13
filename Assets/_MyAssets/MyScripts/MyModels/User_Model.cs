using Newtonsoft.Json;
using Postgrest.Attributes;
using System;

[Table("user")]
public class User_Model
{
    [PrimaryKey("id",false)]
    public long Id { get; set; }

    [Column("user_name")]
    public string UserName { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("surname")]
    public string Surname { get; set; }

    [Column("oauth_id")]
    public string OAuthId { get; set; }

    [Column("oauth_type")]
    public string OAuthType { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; }

    [Column("photo_path")]
    public string PhotoPath { get; set; }

    [Column("virtual_coins")]
    public double? VirtualCoins { get; set; } = 0;

    [Column("xp_points")]
    public long? XpPoints { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column("status")]
    public string Status { get; set; } = "";

    [Column("rating")]
    public float? Rating { get; set; }

    [Column("report_count")]
    public short? ReportCount { get; set; } = 0;

    // Function to convert the object to JSON
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    // Function to create an object from JSON
    public static User_Model FromJson(string json)
    {
        return JsonConvert.DeserializeObject<User_Model>(json);
    }
}
