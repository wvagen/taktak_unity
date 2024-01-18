using System;
using System.Collections;
using System.Collections.Generic;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase.Realtime.Models;
using UnityEngine;

namespace com.mkadmi
{
    [Table("updates")]
    public class Updates_Model : BaseModel
    {
        [Column("ios_version")]
        public int IOS_Version { get; set; } = 2;


        [Column("android_version")]
        public int Android_Version { get; set; }

        [Column("huawei_version")]
        public int Huawei_Version { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
