using System;
using Core;
using UnityEngine;

namespace Game.Service
{
    public class BoundService : IDisposable
    {
        private readonly MessageSystem _messageSystem;
        private readonly float _screenWidth;
        private readonly float _screenHeight;


        private float _leftXPosition;
        private float _rightXPosition;
        private Camera _gameCamera;


        public BoundService(MessageSystem messageSystem)
        {
            _messageSystem = messageSystem;
            _messageSystem.PlayerEvents.OnStartGame += OnStartGame;
            _screenHeight = Screen.height;
            _screenWidth = Screen.width;
        }

        private void OnStartGame()
        {
            _gameCamera = Camera.main;
            Vector3 leftDownPosition = _gameCamera.ScreenToWorldPoint(new Vector3(0,0));
            Vector3 rightTopPosition = _gameCamera.ScreenToWorldPoint(new Vector3(_screenWidth,_screenHeight));
            _leftXPosition = leftDownPosition.x;
            _rightXPosition = rightTopPosition.x;
        }

        public float LeftXPosition => _leftXPosition;

        public float RightXPosition => _rightXPosition;

        public bool IsYDownCamera(float yPosition)
        {
            Vector3 leftDownPosition = _gameCamera.ScreenToWorldPoint(new Vector3(0,0));
            return leftDownPosition.y > yPosition;
        }

        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnStartGame;
        }
    }
}