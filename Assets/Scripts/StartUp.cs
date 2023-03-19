using System;
using System.Collections;
using System.Collections.Generic;
using ClickerTest.Components;
using ClickerTest.Systems;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClickerTest
{
    public class StartUp : MonoBehaviour
    {
        [SerializeField] private InitData gameData;
        [SerializeField] private BusinessParent businessParent;
        [SerializeField] private GameUI gameUI;
        
        private EcsSystems _systems;
        private InitSystem _initSystem;

        private void Start()
        {
            var world = new EcsWorld ();
            _systems = new EcsSystems(world);

            _initSystem = new InitSystem(gameData, businessParent, gameUI);
            
            _systems
                .Add(_initSystem)
                .Add(new TimerSystem())
                .Add(new IncomeSystem())
                .Add(new ProgressSystem())
                .Add(new ViewSystem())
                .Init();
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _initSystem.SaveData();
            }
        }

        private void OnApplicationQuit()
        {
            _initSystem.SaveData();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems?.GetWorld()?.Destroy();
            _systems = null;
        }
    }
}
