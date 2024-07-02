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

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }


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
