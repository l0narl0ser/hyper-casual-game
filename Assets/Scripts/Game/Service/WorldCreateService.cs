using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Game.Controller;
using UniRx;
using UnityEngine;

namespace Game.Service
{
    public class WorldCreateService : IDisposable
    {
        private const string CONTAINER_NAME = "WorldContainer";
        private readonly MessageSystem _messageSystem;
        private readonly CreateControllerService _createControllerService;
        private readonly GameObject _gameWorldRoot;


        private CompositeDisposable _compositeDisposable;
        private PlayerController _playerController;
        private bool _worldExists;
        private List<IRemovable> _allRemovables = new List<IRemovable>();

        public WorldCreateService(MessageSystem messageSystem, CreateControllerService createControllerService)
        {
            _messageSystem = messageSystem;
            _createControllerService = createControllerService;
            _messageSystem.PlayerEvents.OnStartGame += OnGameStarted;
            _gameWorldRoot = GameObject.Find(CONTAINER_NAME);
        }

        private void OnGameStarted()
        {
            _worldExists = true;
            GameObject playerObject = _createControllerService.Create(GameControllerType.Player,
                _gameWorldRoot.transform, new Vector2(0, 4));
            _playerController = playerObject.GetComponent<PlayerController>();

            CreateStartPlatforms();
            CreateBoundChecker();
        }

        private void CreateBoundChecker()
        {
            _compositeDisposable = new CompositeDisposable();
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Subscribe(_ => UpdateBoundPositions())
                .AddTo(_compositeDisposable);
        }

        private void UpdateBoundPositions()
        {
            float playerYPosition = _playerController.transform.position.y;

            foreach (IRemovable removable in _allRemovables.ToList())
            {
                float removablePosition = removable.GetPosition().y;
                if (playerYPosition - removablePosition > 20)
                {
                    removable.Remove();
                    _allRemovables.Remove(removable);
                }
            }
        }

        private void CreateStartPlatforms()
        {
            int yPosition = 0;
            for (int i = 0; i < 15; i++)
            {
                IRemovable platform = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                    _gameWorldRoot.transform,
                    new Vector2(0, yPosition));
                _allRemovables.Add(platform);
                yPosition += 7;
            }
        }

        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnGameStarted;
            _compositeDisposable?.Dispose();
        }

        public PlayerController PlayerController => _playerController;

        public bool WorldExists => _worldExists;
    }
}