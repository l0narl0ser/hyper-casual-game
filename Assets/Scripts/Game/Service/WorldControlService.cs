using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Game.Controller;
using UniRx;
using UnityEngine;

namespace Game.Service
{
    public class WorldControlService : IDisposable
    {

        private const string CONTAINER_NAME = "WorldContainer";
        private readonly MessageSystem _messageSystem;
        private readonly CreateControllerService _createControllerService;
        private readonly GameObject _gameWorldRoot;
        private readonly BoundService _boundService;
        private readonly List<IRemovable> _allRemovables = new List<IRemovable>();
        private LevelGenerator _levelGenerator;
        private Vector2 _lastGeneratedPosition;
        private CompositeDisposable _boundUpdater;
        private PlayerController _playerController;
        private bool _worldExists;

        public WorldControlService(MessageSystem messageSystem, CreateControllerService createControllerService,
            BoundService boundService)
        {
            _messageSystem = messageSystem;
            _boundService = boundService;
            _createControllerService = createControllerService;
            _messageSystem.PlayerEvents.OnStartGame += OnGameStarted;
            _messageSystem.PlayerEvents.OnPlayerDieAnimationFinished += OnPlayerDieAnimationFinished;
            _gameWorldRoot = GameObject.Find(CONTAINER_NAME);
            _levelGenerator = new LevelGenerator(_gameWorldRoot, _boundService, _createControllerService);
        }

        private void OnPlayerDieAnimationFinished()
        {
            foreach (IRemovable removable in _allRemovables)
            {
                removable.Remove();
            }

            _allRemovables.Clear();
            _boundUpdater?.Dispose();
            _playerController.Remove();
            _worldExists = false;
        }

        private void OnGameStarted()
        {
            GameObject playerObject = _createControllerService.Create(GameControllerType.Player,
                _gameWorldRoot.transform, new Vector2(0, 4));
            _playerController = playerObject.GetComponent<PlayerController>();

            _allRemovables.AddRange( _levelGenerator.GenerateStartSpawn());
            _allRemovables.AddRange( _levelGenerator.GenerateObjects());
            CreateBoundChecker();
            _worldExists = true;
        }

        private void CreateBoundChecker()
        {
            _boundUpdater = new CompositeDisposable();
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Subscribe(_ => UpdateBoundPositions())
                .AddTo(_boundUpdater);
        }

        private void UpdateBoundPositions()
        {
            float playerYPosition = _playerController.transform.position.y;

            foreach (IRemovable removable in _allRemovables.ToList())
            {
                float removablePosition = removable.GetPosition().y;
                if (_boundService.IsYDownCamera(removablePosition)) 
                {
                    removable.Remove();
                    _allRemovables.Remove(removable);
                }
            }

            if (_lastGeneratedPosition.y - playerYPosition < 30)
            {
                _allRemovables.AddRange(_levelGenerator.GenerateObjects());
            }
        }


        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnGameStarted;
            _boundUpdater?.Dispose();
        }

        public PlayerController PlayerController => _playerController;

        public bool WorldExists => _worldExists;
    }
}