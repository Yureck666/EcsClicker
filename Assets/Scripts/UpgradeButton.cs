using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickerTest
{
	[RequireComponent(typeof(Button))]
	public class UpgradeButton: MonoBehaviour
	{
		[SerializeField] private TMP_Text name;
		[SerializeField] private TMP_Text description;
		[SerializeField] private TMP_Text cost;

		public float Multiply { get; private set; }
		public bool IsActive { get; private set; }
		private Button _button;

		public void SetUpgradeActive(bool active)
		{
			_button.interactable = !active;
			IsActive = active;
		}

		public void Init(UpgradeScriptable config, Action<float, uint, Action> onClick)
		{
			_button = GetComponent<Button>();
			Multiply = config.IncomeMultiply;
			_button.onClick.AddListener(() =>
			{
				onClick.Invoke(Multiply, config.Cost, () => SetUpgradeActive(true));
			});
			name.text = config.Name;
			description.text = $"{config.Description}{(Multiply - 1) * 100}%";
			cost.text = $"{config.Cost}$";
			
			SetUpgradeActive(false);
		}
	}
}