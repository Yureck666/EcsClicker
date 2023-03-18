using UnityEngine;

namespace ClickerTest
{
	[CreateAssetMenu(fileName = "BusinessConfig", menuName = "Configs/Business")]
	public class BusinessScriptable: ScriptableObject
	{
		[SerializeField] private Business businessPrefab;
		[SerializeField] private uint startLevel;
		[SerializeField] private string name;
		[SerializeField] private uint basicIncome;
		[SerializeField] private uint cost;
		[SerializeField] private float incomeDelay;
		[SerializeField] private UpgradeScriptable[] upgrades;

		public Business BusinessPrefab => businessPrefab;
		public uint StartLevel => startLevel;
		public string Name => name;
		public uint BasicIncome => basicIncome;
		public uint Cost => cost;
		public float IncomeDelay => incomeDelay;
		public UpgradeScriptable[] Upgrades => upgrades;

		public int Entity { get; private set; }

		public void SetEntity(int value)
		{
			Entity = value;
		}
	}
}