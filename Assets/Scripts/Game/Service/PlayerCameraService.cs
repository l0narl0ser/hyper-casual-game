using System;
using Core;
using Game.Controller;
using UniRx;
using UnityEngine;

namespace Game.Service
{
    public class PlayerCameraService : IDisposable
    {
        private static readonly Vector3 VectorCameraStartPosition = new Vector3(0, 0, -10);
        private const float CameraSpeed = 0.3f;
        private readonly WorldCreateService _worldCreateService;
        private readonly MessageSystem _messageSystem;
        private Camera _gameCamera;
        private CompositeDisposable _lateDisposable;
        

        public PlayerCameraService(WorldCreateService worldCreateService, MessageSystem messageSystem)
        {
             _worldCreateService = worldCreateService;
            _messageSystem = messageSystem;
            _messageSystem.PlayerEvents.OnStartGame += OnGameStarted;
        }

        private void OnGameStarted()
        {
            _gameCamera = Camera.main;
            _gameCamera.transform.position = VectorCameraStartPosition;
            _lateDisposable = new CompositeDisposable();
            
            Observable.EveryLateUpdate()
                .Where(_ => _worldCreateService.WorldExists)
                .Subscribe(_ =>  UpdateCameraPosition())
                .AddTo(_lateDisposable);
        }

        private void UpdateCameraPosition()
        {
            PlayerController playerController = _worldCreateService.PlayerController;
            if (playerController.transform.position.y < _gameCamera.transform.position.y)
            {
                return;
            }
            Vector3 position = _gameCamera.transform.position;
            Vector3 newPosition = new Vector3(position.x,
                playerController.transform.position.y, position.z);
                
            position = Vector3.Lerp(position, newPosition, CameraSpeed);
                
            _gameCamera.transform.position = position;
        }

        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnGameStarted;
            _lateDisposable?.Dispose();
        }
    }
}