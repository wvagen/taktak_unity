using System;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;
using UnityEngine;

namespace com.mkadmi
{
    [Table("company")]
    public class Company_Model : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("number")]
        public long Number { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; }

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

        public Company_Model()
        {

        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Company_Model FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Company_Model>(json);
        }
    }
}
