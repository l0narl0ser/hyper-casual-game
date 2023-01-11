using System;
using Core;
using UnityEngine;

namespace Game.Service
{
    public class PauseService : IDisposable
    {
        private MessageSystem _messageSystem;

        public PauseService(MessageSystem messageSystem)
        {
            _messageSystem = messageSystem;
            _messageSystem.PlayerEvents.OnPlayerPaused += OnPlayerPaused;
            _messageSystem.PlayerEvents.OnPlayerUnpaused += OnPlayerUnpaused;
        }

        private void OnPlayerUnpaused()
        {
            Time.timeScale = 1;
        }

        private void OnPlayerPaused()
        {
            Time.timeScale = 0;
        }


        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnPlayerPaused -= OnPlayerPaused;
            _messageSystem.PlayerEvents.OnPlayerUnpaused -= OnPlayerUnpaused;
        }
    }
}