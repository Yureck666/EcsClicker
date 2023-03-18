using System;
using ClickerTest.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClickerTest.Systems
{
	public class IncomeSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private EcsPool<IncomeComponent> _incomePool;
		private EcsPool<LevelComponent> _levelPool;
		private EcsPool<MultiplyComponent> _multiplyPool;
		private EcsPool<TimerComponent> _timerPool;
		private EcsPool<MoneyComponent> _moneyPool;
		private EcsPool<ReadyToIncomeComponent> _readyToIncomePool;
		private EcsFilter _moneyFilter;
		private EcsFilter _incomeFilter;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			_incomeFilter = _world
				.Filter<ReadyToIncomeComponent>()
				.End();

			_moneyFilter = _world
				.Filter<MoneyComponent>()
				.End();

			_timerPool = _world.GetPool<TimerComponent>();
			_incomePool = _world.GetPool<IncomeComponent>();
			_levelPool = _world.GetPool<LevelComponent>();
			_multiplyPool = _world.GetPool<MultiplyComponent>();
			_readyToIncomePool = _world.GetPool<ReadyToIncomeComponent>();

			_moneyPool = _world.GetPool<MoneyComponent>();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var moneyEntity in _moneyFilter)
			{
				ref var money = ref _moneyPool.Get(moneyEntity);

				foreach (var incomeEntity in _incomeFilter)
				{
					ref var timer = ref _timerPool.Get(incomeEntity);
					ref var income = ref _incomePool.Get(incomeEntity);
					ref var level = ref _levelPool.Get(incomeEntity);
					ref var multiply = ref _multiplyPool.Get(incomeEntity);
					
					_readyToIncomePool.Del(incomeEntity);
					timer.Progress = 0;
					uint incomeValue = Convert.ToUInt32((income.Income * level.Level) * multiply.Multiply);
					money.Money += incomeValue;
				}
			}
		}
	}
}