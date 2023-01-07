﻿using System;
using Core;
using Game.Controller;
using UnityEngine;

namespace Game.Service
{
    public class WorldCreateService : IDisposable
    {
        private const string CONTAINER_NAME = "WorldContainer";
        private readonly MessageSystem _messageSystem;
        private readonly CreateControllerService _createControllerService;
        private GameObject _gameWorldRoot;


        public WorldCreateService(MessageSystem messageSystem, CreateControllerService createControllerService)
        {
            _messageSystem = messageSystem;
            _createControllerService = createControllerService;
            _messageSystem.PlayerEvents.OnStartGame += OnGameStarted;
            _gameWorldRoot = GameObject.Find(CONTAINER_NAME);
        }

        private void OnGameStarted()
        {
            _createControllerService.Create(GameControllerType.Player, _gameWorldRoot.transform, new Vector2(0, 0));
            _createControllerService.Create(GameControllerType.StandardPlatform, _gameWorldRoot.transform,
                new Vector2(0, -3));
            _createControllerService.Create(GameControllerType.StandardPlatform, _gameWorldRoot.transform,
                new Vector2(0, 5));
        }

        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnGameStarted;
        }
    }
}