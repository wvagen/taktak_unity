using System;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;
using UnityEngine;

namespace com.mkadmi
{
    [Table("committer")]
    public class Committer_Model : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("mission_id")]
        public long MissionId { get; set; }

        [Column("committer_id")]
        public string CommitterId { get; set; }

        [Column("patent")]
        public string Patent { get; set; }

        [Column("photo_path")]
        public string PhotoPath { get; set; }

        [Column("total_coins_spent")]
        public long TotalCoinsSpent { get; set; }

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

        public Sprite CompanySprite{ get; set; }

        public Committer_Model()
        {

        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Committer_Model FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Committer_Model>(json);
        }
    }
}
