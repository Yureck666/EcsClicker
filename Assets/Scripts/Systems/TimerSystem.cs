using ClickerTest.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClickerTest.Systems
{
	public class TimerSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private EcsPool<TimerComponent> _timerPool;
		private EcsFilter _timerFilter;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			_timerFilter = _world.Filter<TimerComponent>().End();
			
			_timerPool = _world.GetPool<TimerComponent>();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var timerEntity in _timerFilter)
			{
				ref var timer = ref _timerPool.Get(timerEntity);
				var readyToIncomePool = _world.GetPool<ReadyToIncomeComponent>();
				
				timer.Progress += Time.deltaTime;
				
				if (timer.Progress >= timer.Time && !readyToIncomePool.Has(timerEntity))
				{
					readyToIncomePool.Add(timerEntity);
				}
			}
		}
	}
}