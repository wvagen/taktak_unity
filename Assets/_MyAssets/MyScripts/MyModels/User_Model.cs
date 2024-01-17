using System;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace com.mkadmi
{
    [Table("user")]
    public class User_Model : BaseModel
    {
        [PrimaryKey("id",false)]
        public string Id { get; set; }

        [Column("number")]
        public long Number { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("oauth_id")]
        public string OauthId { get; set; }

        [Column("oauth_type")]
        public string OauthType { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("photo_path")]
        public string PhotoPath { get; set; }

        [Column("virtual_coins")]
        public long VirtualCoins { get; set; }

        [Column("xp_points")]
        public ulong XpPoints { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("rating")]
        public float? Rating { get; set; }

        [Column("report_count")]
        public short? ReportCount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public User_Model()
        {

        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static User_Model FromJson(string json)
        {
            return JsonConvert.DeserializeObject<User_Model>(json);
        }
    }
}
