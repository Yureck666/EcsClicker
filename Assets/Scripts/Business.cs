using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickerTest
{
	public class Business : MonoBehaviour
	{
		[SerializeField] private new TMP_Text name;
		[SerializeField] private Button levelUpButton;
		[SerializeField] private TMP_Text levelUpCost;
		[SerializeField] private TMP_Text level;
		[SerializeField] private TMP_Text income;
		[SerializeField] private UpgradeButton[] upgradeButtons;
		[SerializeField] private Slider slider;

		public Slider Slider => slider;
		public uint BasicCost { get; private set; }
		public float IncomeDelay { get; private set; }
		public int Entity { get; private set; }

		public void SetEntity(int value)
		{
			Entity = value;
		}
		public void SetIncome(uint value)
		{
			income.text = value.ToString();
		}

		public void SetLevel(uint value)
		{
			level.text = value.ToString();
			SetLevelUpCost(value);
		}

		public void SetLevelUpCost(uint level)
		{
			levelUpCost.text = $"{(level+1) * BasicCost}$";
		}

		public void SetUpgrades(bool[] active, Action<float> multiplyAction)
		{
			for (int i = 0; i < active.Length; i++)
			{
				if (upgradeButtons.Length > i && active[i])
				{
					var button = upgradeButtons[i];
					button.SetUpgradeActive(true);
					multiplyAction.Invoke(button.Multiply);
				}
			}
		}

		public bool[] GetButtonsActive()
		{
			var array = new bool[upgradeButtons.Length];
			for (int i = 0; i < upgradeButtons.Length; i++)
			{
				array[i] = upgradeButtons[i].IsActive;
			}

			return array;
		}

		public void Init(BusinessScriptable config, Action levelUp, Action<float, uint, Action> upgradeAction)
		{
			BasicCost = config.Cost;
			IncomeDelay = config.IncomeDelay;
			SetLevel(0);
			name.text = config.Name;
			levelUpButton.onClick.AddListener(levelUp.Invoke);
			for (int i = 0; i < config.Upgrades.Length; i++)
			{
				var upgradeButton = upgradeButtons[i];
				upgradeButton.Init(config.Upgrades[i], upgradeAction);
			}
		}
	}
}