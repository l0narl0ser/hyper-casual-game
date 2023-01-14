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
        private const float DistanceToShowLooseDialog = 50f;
        private readonly WorldControlService _worldControlService;
        private readonly MessageSystem _messageSystem;
        
        private Camera _gameCamera;
        private CompositeDisposable _lateDisposable;
        private float _playerStartDyingYPosition;
        

        public PlayerCameraService(WorldControlService worldControlService, MessageSystem messageSystem)
        {
             _worldControlService = worldControlService;
            _messageSystem = messageSystem;
            _messageSystem.PlayerEvents.OnStartGame += OnGameStarted;
            _messageSystem.PlayerEvents.OnPlayerDead += OnPlayerDead;
        }

        private void OnPlayerDead()
        {
            _playerStartDyingYPosition = _worldControlService.PlayerController.gameObject.transform.position.y;
        }

        private void OnGameStarted()
        {
            _gameCamera = Camera.main;
            _gameCamera.transform.position = VectorCameraStartPosition;
            _lateDisposable = new CompositeDisposable();
            
            Observable.EveryLateUpdate()
                .Where(_ => _worldControlService.WorldExists)
                .Subscribe(_ =>  UpdateCameraPosition())
                .AddTo(_lateDisposable);
        }

        private void UpdateCameraPosition()
        {
            PlayerController playerController = _worldControlService.PlayerController;

            if (playerController.PlayerDead)
            {
                PlayerDeadCameraMoving(playerController);
            }
            else
            {
                PlayerGameplayCameraMoving(playerController);
            }
            
        }

        private void PlayerDeadCameraMoving(PlayerController playerController)
        {
            if (_playerStartDyingYPosition - playerController.transform.position.y > DistanceToShowLooseDialog)
            {
                FinishCameraMove();
            }
            else
            {
                MoveCamera(playerController);
            }
        }

        private void FinishCameraMove()
        {
            _lateDisposable?.Dispose();
            _messageSystem.PlayerEvents.FinishDieAnimation();
        }

        private void PlayerGameplayCameraMoving(PlayerController playerController)
        {
            if (playerController.transform.position.y < _gameCamera.transform.position.y)
            {
                return;
            }

            MoveCamera(playerController);
        }


        private void MoveCamera(PlayerController playerController)
        {
            Vector3 position = _gameCamera.transform.position;
            Vector3 newPosition = new Vector3(position.x,
                playerController.transform.position.y, position.z);

            position = Vector3.Lerp(position, newPosition, CameraSpeed);

            _gameCamera.transform.position = position;
        }

        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnGameStarted;
            _messageSystem.PlayerEvents.OnPlayerDead -= OnPlayerDead;
            _lateDisposable?.Dispose();
        }

        public Camera GetGameCamera => _gameCamera;
    }
}