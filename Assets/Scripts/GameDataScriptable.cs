using UnityEngine;

namespace ClickerTest
{
	[CreateAssetMenu(fileName = "GameData", menuName = "Configs/GameData")]
	public class GameDataScriptable: ScriptableObject
	{
		[SerializeField] private BusinessScriptable[] businesses;

		public BusinessScriptable[] Businesses => businesses;
	}
}