using UnityEngine.Events;

namespace ClickerTest
{
	public class GameModel
	{
		public uint Money
		{
			get => _money;
			set
			{
				_money = value;
				OnMoneyChange.Invoke(_money);
			}
		}

		public UnityEvent<uint> OnMoneyChange { get; }

		private uint _money;
		
		public GameModel()
		{
			OnMoneyChange = new UnityEvent<uint>();
			Money = 0;
		}

		public bool SubtractMoney(uint value)
		{
			if (value > Money)
				return false;
			
			Money -= value;
			return true;
		}

		public void AddMoney(uint value)
		{
			Money += value;
		}
	}
}