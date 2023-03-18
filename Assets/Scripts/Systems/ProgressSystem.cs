using ClickerTest.Components;
using Leopotam.EcsLite;

namespace ClickerTest.Systems
{
	public class ProgressSystem : IEcsInitSystem, IEcsRunSystem
	{
		private EcsWorld _world;
		private EcsPool<TimerComponent> _timerPool;
		private EcsPool<ProgressComponent> _progressPool;
		private EcsFilter _filter;

		public void Init(IEcsSystems systems)
		{
			_world = systems.GetWorld();

			_filter = _world
				.Filter<TimerComponent>()
				.Inc<ProgressComponent>()
				.End();

			_timerPool = _world.GetPool<TimerComponent>();
			_progressPool = _world.GetPool<ProgressComponent>();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var timer = _timerPool.Get(entity);
				var progress = _progressPool.Get(entity);

				progress.Slider.value = timer.Progress != 0 ? timer.Progress / timer.Time : 0;
			}
		}
	}
}