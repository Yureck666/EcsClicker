using System;
using System.Collections.Generic;
using System.Linq;
using ClickerTest.Components;
using Code.StorageObjects;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Events;

namespace ClickerTest
{
	public class InitSystem : IEcsInitSystem
	{
		private InitData _gameData;
		private BusinessParent _businessParent;
		private GameUI _gameUI;
		private EcsWorld _world;
		private int _counterEntity;
		private int _viewEntity;
		private StorageObject _storageObject;
		private Business[] _businesses;

		public InitSystem(InitData gameData, BusinessParent businessParent, GameUI gameUI)
		{
			_gameData = gameData;
			_businessParent = businessParent;
			_gameUI = gameUI;
		}

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_businesses = new Business[_gameData.BusinessScriptable.Length];

			LoadData();

			_counterEntity = _world.NewEntity();
			ref var money = ref AddComponent<MoneyComponent>(_counterEntity);
			money.Money = _storageObject.Data.Money;

			_viewEntity = _world.NewEntity();
			ref var view = ref AddComponent<GameViewComponent>(_viewEntity);
			view.MoneySetter = _gameUI.ChangeMoneyText;

			_gameUI.ChangeMoneyText(money.Money);

			CreateBusiness();
		}

		public void SaveData()
		{
			_storageObject = new StorageObject();
			_storageObject.Data = new StorageObject.GameData();

			ref var money = ref _world.GetPool<MoneyComponent>().Get(_counterEntity);
			_storageObject.Data.Money = money.Money;

			foreach (var business in _businesses)
			{
				var businessEntity = business.Entity;

				ref var level = ref _world.GetPool<LevelComponent>().Get(businessEntity);
				var data = new StorageObject.BusinessData(level.Level);
				data.Upgrades = business.GetButtonsActive().ToList();

				var timerPool = _world.GetPool<TimerComponent>();
				if (timerPool.Has(businessEntity))
				{
					data.Progress = timerPool.Get(businessEntity).Progress;
				}
				_storageObject.Data.BusinessesData.Add(data);
			}

			PersistentStorage.PersistentStorage.Save<StorageObject, StorageObject.GameData>(_storageObject);
		}

		private void CreateBusiness()
		{
			var savedBusinesses = _storageObject.Data.BusinessesData;
			for (int i = 0; i < _gameData.BusinessScriptable.Length; i++)
			{
				var businessData = _gameData.BusinessScriptable[i];
				var savedData = savedBusinesses.Count > i ? savedBusinesses[i] : new StorageObject.BusinessData(businessData.StartLevel);
				var prefab = businessData.BusinessPrefab;
				var businessEntity = _world.NewEntity();
				ref var income = ref AddComponent<IncomeComponent>(businessEntity);
				ref var multiply = ref AddComponent<MultiplyComponent>(businessEntity);
				ref var level = ref AddComponent<LevelComponent>(businessEntity);
				ref var progress = ref AddComponent<ProgressComponent>(businessEntity);
				ref var businessView = ref AddComponent<BusinessViewComponent>(businessEntity);

				var businessObject = GameObject.Instantiate(prefab, _businessParent.transform);
				businessObject.SetEntity(businessEntity);
				businessObject.Init(
					businessData,
					() => LevelUp(businessObject),
					(mp, cost, disableAction) => AddUpgrade(businessEntity, mp, cost, disableAction));

				_businesses[i] = businessObject;

				income.Income = businessData.BasicIncome;
				multiply.Multiply = 1;
				progress.Slider = businessObject.Slider;
				businessView.IncomeSetter = businessObject.SetIncome;
				businessView.LevelSetter = businessObject.SetLevel;
				level.Level = savedData.Level;
				
				businessObject.SetUpgrades(savedData.Upgrades.ToArray(), mp => AddMultiply(businessEntity, mp));

				var timerPool = _world.GetPool<TimerComponent>();

				if (level.Level > 0)
				{
					timerPool.Add(businessEntity);
					ref var timer = ref timerPool.Get(businessEntity);
					timer.Progress = savedData.Progress;
					timer.Time = businessData.IncomeDelay;

					AddComponent<RefreshBusinessViewEventComponent>(businessEntity);
				}
			}
		}

		private void LoadData()
		{
			_storageObject = new StorageObject();
			_storageObject = PersistentStorage.PersistentStorage.Load<StorageObject, StorageObject.GameData>(new StorageObject());
		}

		private void LevelUp(Business data)
		{
			var entity = data.Entity;
			var cost = (GetCurrentLevel(entity) + 1) * data.BasicCost;
			CheckMoneyAndInvoke(() =>
			{
				var timerPool = _world.GetPool<TimerComponent>();
				if (!timerPool.Has(entity))
				{
					timerPool.Add(entity);
					ref var timer = ref timerPool.Get(entity);
					timer.Progress = 0;
					timer.Time = data.IncomeDelay;
				}

				ref var levelComponent = ref _world.GetPool<LevelComponent>().Get(entity);
				levelComponent.Level++;

				AddComponent<RefreshBusinessViewEventComponent>(entity);
			}, cost);
		}

		private uint GetCurrentLevel(int entity)
		{
			return _world.GetPool<LevelComponent>().Get(entity).Level;
		}

		private void AddUpgrade(int entity, float multiply, uint cost, Action disableAction)
		{
			CheckMoneyAndInvoke(() =>
			{
				AddMultiply(entity, multiply);
				disableAction.Invoke();
				AddComponent<RefreshBusinessViewEventComponent>(entity);

			}, cost);
		}

		private void AddMultiply(int entity, float value)
		{
			ref var multiplyComponent = ref _world.GetPool<MultiplyComponent>().Get(entity);
			multiplyComponent.Multiply *= value;
		}

		private void InvokeChangeMoneyEvent(uint value)
		{
			ref var viewComponent = ref _world.GetPool<GameViewComponent>().Get(_viewEntity);
			viewComponent.MoneySetter.Invoke(value);
		}

		private void CheckMoneyAndInvoke(Action action, uint cost)
		{
			ref var moneyComponent = ref _world.GetPool<MoneyComponent>().Get(_counterEntity);
			var moneyCount = moneyComponent.Money;
			if (moneyCount >= cost)
			{
				moneyComponent.Money -= cost;
				InvokeChangeMoneyEvent(moneyComponent.Money);
				action.Invoke();
			}
		}

		private ref T AddComponent<T>(int entity) where T : struct
		{
			var pool = _world.GetPool<T>();
			ref var component = ref pool.Add(entity);
			return ref component;
		}
	}
}