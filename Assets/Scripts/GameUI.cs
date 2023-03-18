using TMPro;
using UnityEngine;

namespace ClickerTest
{
	public class GameUI: MonoBehaviour
	{
		[SerializeField] private TMP_Text moneyCount;
		
		public void ChangeMoneyText(uint value)
		{
			moneyCount.text = $"{value}$";
		}
	}
}