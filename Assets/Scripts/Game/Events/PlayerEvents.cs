using System;

namespace Game.Events
{
    public class PlayerEvents
    {
        public event Action OnStartGame;
        public event Action OnPlayerDead;
        public event Action OnPlayerPaused;
        public event Action OnPlayerUnpaused;
        public event Action OnPlayerDieAnimationFinished;


        public void FinishDieAnimation()
        {
            OnPlayerDieAnimationFinished?.Invoke();
        }

        public void StartGame()
        {
            OnStartGame?.Invoke();
        }

        public void PlayerDead()
        {
            OnPlayerDead?.Invoke();
        }

        public void PauseGame()
        {
            OnPlayerPaused?.Invoke();
        }

        public void UnpauseGame()
        {
            OnPlayerUnpaused?.Invoke();
        }
    }
}