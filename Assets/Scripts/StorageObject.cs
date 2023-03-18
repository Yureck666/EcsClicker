using System;
using System.Collections.Generic;
using ClickerTest.Components;
using PersistentStorage;
using UnityEngine;

namespace Code.StorageObjects
{
    public class StorageObject : PlainStorageObject<StorageObject.GameData>
    {
        [Serializable]
        public class GameData
        {
            public uint Money;
            public List<BusinessData> BusinessesData;

            public GameData()
            {
                BusinessesData = new List<BusinessData>();
            }
        }
        
        public class BusinessData
        {
            public uint Level;
            public float Progress;
            public List<bool> Upgrades;
            public BusinessData(uint level)
            {
                Upgrades = new List<bool>();
                Level = level;
            }
        }

        public StorageObject(Action<GameData> afterLoading = null, Func<GameData> beforeSaving = null) : base(new GameData()
        {
            Money = 0,
            BusinessesData = new List<BusinessData>()
        }, afterLoading, beforeSaving)
        {
        }

        public override string PrefKey => nameof(StorageObject);
    }
}