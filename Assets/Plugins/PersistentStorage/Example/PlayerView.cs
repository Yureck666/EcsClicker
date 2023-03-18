using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PersistentStorage.Example
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField healthPlace;
        [SerializeField] private TMP_InputField armorPlace;
        [SerializeField] private TMP_InputField moneyPlace;

        [SerializeField] private TMP_Text achievementPrototype;
        [SerializeField] private Button addAchievementButton;
        
        private List<GameObject> _achievements;
        private PlayerStorageObject.PlayerData _playerData;

        private void Awake()
        {
            addAchievementButton.onClick.AddListener(AddAchievement);
        }

        public void Initialize(PlayerStorageObject.PlayerData data)
        {
            _playerData = data;
            healthPlace.text = $"{_playerData.Health:000}";
            armorPlace.text = $"{_playerData.Armor:00}";
            
            moneyPlace.text = $"{_playerData.Money:000}";

            if (_achievements == null)
            {
                _achievements = new List<GameObject>();
            }
            else
            {
                _achievements.ForEach(Destroy);
                _achievements.Clear();
            }

            foreach (var achievement in _playerData.Achievements)
            {
                _achievements.Add(CreateAchievement(achievement));
            }
        }

        private void AddAchievement()
        {
            var n = _playerData.Achievements.Count + 1;
            var ach = $"Ach {n}";
            
            _playerData.Achievements.Add(ach);
            CreateAchievement(ach);
        }
        
        private GameObject CreateAchievement(string text)
        {
            var instance = Instantiate(achievementPrototype.gameObject, achievementPrototype.transform.parent).GetComponent<TMP_Text>();
            instance.text = $"* {text}";
            
            var go = instance.gameObject;
            go.SetActive(true);
            
            return go;
        }

        public PlayerStorageObject.PlayerData Parse()
        {
            if (float.TryParse(healthPlace.text, out var health))
                _playerData.Health = health;
            
            if (float.TryParse(armorPlace.text, out var armor))
                _playerData.Armor = armor;
            
            if (int.TryParse(moneyPlace.text, out var money))
                _playerData.Money = money;

            return _playerData;
        }
    }
}