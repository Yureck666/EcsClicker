using UnityEngine;

namespace ClickerTest
{

	[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/Upgrade")]
	public class UpgradeScriptable : ScriptableObject
	{
		[SerializeField] private string name;
		[SerializeField] private uint cost;
		[SerializeField] private string description;
		[SerializeField] private float incomeMultiply;

		public string Name => name;
		public uint Cost => cost;
		public string Description => description;
		public float IncomeMultiply => incomeMultiply;
	}
}