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

        [Column("company_id")]
        public long CompanyID { get; set; }

        [Column("item_price")]
        public float ItemPrice { get; set; }

        [Column("item_photo_path")]
        public string ItemPhotoPath { get; set; }

        [Column("reward_virt_coins")]
        public int RewardVirtCoins { get; set; }

        [Column("reward_xp")]
        public int RewardXp { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("title_fr")]
        public string TitleFr { get; set; }

        [Column("title_en")]
        public string TitleEn { get; set; }

        [Column("title_ar")]
        public string TitleAr { get; set; }

        [Column("title_tn")]
        public string TitleTn { get; set; }

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
