using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Game.Controller;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Service
{
    public class WorldCreateService : IDisposable
    {
        private const string CONTAINER_NAME = "WorldContainer";
        private readonly MessageSystem _messageSystem;
        private readonly CreateControllerService _createControllerService;
        private readonly GameObject _gameWorldRoot;
        private readonly BoundService _boundService;

        private Vector2 _lastGeneratedPlatformPosition;
        private CompositeDisposable _compositeDisposable;
        private PlayerController _playerController;
        private bool _worldExists;
        private List<IRemovable> _allRemovables = new List<IRemovable>();

        public WorldCreateService(MessageSystem messageSystem, CreateControllerService createControllerService,
            BoundService boundService)
        {
            _messageSystem = messageSystem;
            _boundService = boundService;
            _createControllerService = createControllerService;
            _messageSystem.PlayerEvents.OnStartGame += OnGameStarted;
            _gameWorldRoot = GameObject.Find(CONTAINER_NAME);
        }

        private void OnGameStarted()
        {

            GameObject playerObject = _createControllerService.Create(GameControllerType.Player,
                _gameWorldRoot.transform, new Vector2(0, 4));
            _playerController = playerObject.GetComponent<PlayerController>();
            _lastGeneratedPlatformPosition = new Vector2(0, -10);
            
            GeneratePlatforms();
            CreateBoundChecker();
            
            _worldExists = true;
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
                if (playerYPosition - removablePosition > 20) //TODO : Вынести в константы
                {
                    removable.Remove();
                    _allRemovables.Remove(removable);
                }
            }
            //TODO:// ВЫнести в константы и зависит от минимальнной позиции по рандомному игрику

            if (_lastGeneratedPlatformPosition.y - playerYPosition < 30)
            {
                GeneratePlatforms();
            }
        }


        public void GeneratePlatforms()
        {
            int roundedLeftPosition = Mathf.RoundToInt(_boundService.LeftXPosition) + 3; //TODO : в константы
            int roundedRightPosition = Mathf.RoundToInt(_boundService.RightXPosition) - 3;

            for (int i = 0; i < 10; i++)
            {
                int deltaYPosition = Random.Range(3, 7); // TODO вынести в кностанты
                int xPosition = Random.Range(roundedLeftPosition, roundedRightPosition);

                _lastGeneratedPlatformPosition =
                    new Vector2(xPosition, _lastGeneratedPlatformPosition.y + deltaYPosition);
                IRemovable platform = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                    _gameWorldRoot.transform, _lastGeneratedPlatformPosition);
                _allRemovables.Add(platform);
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