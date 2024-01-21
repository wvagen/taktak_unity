using System;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace com.mkadmi
{
    [Table("mission_live")]
    public class MissionLive_Model : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("requester_id")]
        public string RequesterId { get; set; }

        [Column("committers_id")]
        public string[] CommittersId { get; set; }

        [Column("reward_virt_coins")]
        public long RewardVirtCoins { get; set; }

        [Column("reward_xp")]
        public long RewardXp { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("deadline")]
        public DateTime Deadline { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("decided_user_id")]
        public string DecidedUserId { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static MissionLive_Model FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MissionLive_Model>(json);
        }
    }
}
