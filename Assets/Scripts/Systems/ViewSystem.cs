using System;
using ClickerTest.Components;
using Leopotam.EcsLite;

namespace ClickerTest.Systems
{
	public class ViewSystem: IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private EcsPool<IncomeComponent> _incomePool;
		private EcsPool<LevelComponent> _levelPool;
		private EcsPool<MultiplyComponent> _multiplyPool;
		private EcsPool<BusinessViewComponent> _businessViewPool;
		private EcsPool<GameViewComponent> _viewPool;
		private EcsPool<MoneyComponent> _moneyPool;
		private EcsPool<RefreshBusinessViewEventComponent> _refreshBusinessViewPool;
		private EcsFilter _refreshBusinessViewFilter;
		private EcsFilter _moneyFilter;
		private EcsFilter _gameViewFilter;
		
		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			
			_refreshBusinessViewFilter = _world
				.Filter<RefreshBusinessViewEventComponent>()
				.End();
			
			_gameViewFilter = _world
				.Filter<GameViewComponent>()
				.End();
			
			_moneyFilter = _world
				.Filter<MoneyComponent>()
				.End();

			_incomePool = _world.GetPool<IncomeComponent>();
			_levelPool = _world.GetPool<LevelComponent>();
			_multiplyPool = _world.GetPool<MultiplyComponent>();
			_moneyPool = _world.GetPool<MoneyComponent>();
			_businessViewPool = _world.GetPool<BusinessViewComponent>();
			_refreshBusinessViewPool = _world.GetPool<RefreshBusinessViewEventComponent>();
			_viewPool = _world.GetPool<GameViewComponent>();
		}
		
		public void Run(IEcsSystems systems)
		{
			foreach (var viewEntity in _refreshBusinessViewFilter)
			{
				ref var income = ref _incomePool.Get(viewEntity);
				ref var level = ref _levelPool.Get(viewEntity);
				ref var multiply = ref _multiplyPool.Get(viewEntity);
				ref var view = ref _businessViewPool.Get(viewEntity);
				
				uint incomeValue = Convert.ToUInt32((income.Income * level.Level) * multiply.Multiply);
				view.IncomeSetter.Invoke(incomeValue);
				view.LevelSetter.Invoke(level.Level);
				
				_refreshBusinessViewPool.Del(viewEntity);
			}

			foreach (var moneyEntity in _moneyFilter)
			{
				ref var money = ref _moneyPool.Get(moneyEntity);
				
				foreach (var viewEntity in _gameViewFilter)
				{
					ref var view = ref _viewPool.Get(viewEntity);
					
					view.MoneySetter.Invoke(money.Money);
				}
			}
		}
	}
}