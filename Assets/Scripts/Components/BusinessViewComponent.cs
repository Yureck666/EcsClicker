using System;

namespace ClickerTest.Components
{
	public struct BusinessViewComponent
	{
		public Action<uint> LevelSetter;
		public Action<uint> IncomeSetter;
	}
}