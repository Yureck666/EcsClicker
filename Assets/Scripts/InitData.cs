using UnityEngine;

namespace ClickerTest
{

	[CreateAssetMenu(fileName = "InitData")]
	public class InitData : ScriptableObject
	{
		[SerializeField] private uint startMoney;
		[SerializeField] private BusinessScriptable[] businessScriptable;

		public uint StartMoney => startMoney;
		public BusinessScriptable[] BusinessScriptable => businessScriptable;
	}
}