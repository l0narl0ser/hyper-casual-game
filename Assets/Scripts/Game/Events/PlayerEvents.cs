using System;

namespace Game.Events
{
    public class PlayerEvents
    {
        public event Action OnStartGame;
        

        public void StartGame()
        {
            OnStartGame?.Invoke();
        }
    }
}