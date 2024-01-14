using System;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace com.mkadmi
{
    [Table("user_id_map")]
    public class UserIdMap_Model : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("user_cred")]
        public string UserCred { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("user_type")]
        public string UserType { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("upadted_at")]
        public DateTime? UpdatedAt { get; set; } = null;

        public UserIdMap_Model()
        {

        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static UserIdMap_Model FromJson(string json)
        {
            return JsonConvert.DeserializeObject<UserIdMap_Model>(json);
        }
    }
}
