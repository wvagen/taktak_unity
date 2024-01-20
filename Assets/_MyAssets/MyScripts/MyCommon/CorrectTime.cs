using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace com.mkadmi
{
    public class CorrectTime : MonoBehaviour
    {
        [Serializable]
        public class WorldAPI
        {
            public string utc_datetime;
            public int raw_offset;
            public string datetime;
            public ulong unixtime;
        }

        /* API (json)
    {
        "abbreviation" : "+01",
        "client_ip"    : "190.107.125.48",
        "datetime"     : "2020-08-14T15:544:04+01:00",
        "dst"          : false,
        "dst_from"     : null,
        "dst_offset"   : 0,
        "dst_until"    : null,
        "raw_offset"   : 3600,
        "timezone"     : "Asia/Brunei",
        "unixtime"     : 1595601262,
        "utc_datetime" : "2020-08-14T15:54:04+00:00",
        "utc_offset"   : "+01:00"
    }
    We only need "datetime" property.*/

        static CorrectTime _instance;

        public WorldAPI time = new WorldAPI();

        double realTime;


        public ulong realTimeStamp;
        public DateTime realDate;

        bool canStartTime = false;

        private void Awake()
        {
            StartCoroutine(FetchTime());
        }

        private void FixedUpdate()
        {
            if (canStartTime)
            {
                realTime += Time.fixedDeltaTime;
                realTimeStamp = (ulong)realTime;
                realDate = Tools.Convert_To_DateTime(realTimeStamp);
            }
        }

        public static CorrectTime Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CorrectTime>();
            }
            return _instance;
        }

        IEnumerator FetchTime()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(UserSettings.TIME_API))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    time = JsonUtility.FromJson<WorldAPI>(request.downloadHandler.text);
                    canStartTime = true;
                    realTime = time.unixtime;
                }
            }

        }
    }
}