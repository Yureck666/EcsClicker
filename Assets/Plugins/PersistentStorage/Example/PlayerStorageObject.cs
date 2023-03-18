using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PersistentStorage.Example
{
    public class PlayerStorageObject : PlainStorageObject<PlayerStorageObject.PlayerData>
    {
        public override string PrefKey => nameof(PlayerData);

        [Serializable]
        public class PlayerData
        {
            [field:SerializeField]
            [JsonProperty] public float Health { get; set; }
            [field:SerializeField]
            [JsonProperty] public float Armor { get; set; }
            
            [field:SerializeField]
            [JsonProperty] public int Money { get; set; }
            
            [field:SerializeField]
            [JsonProperty] public List<string> Achievements { get; set; }
        }

        public PlayerStorageObject(PlayerData defaultData, Action<PlayerData> afterLoading = null, Func<PlayerData> beforeSaving = null) : base(defaultData, afterLoading, beforeSaving)
        {
        }
    }
}