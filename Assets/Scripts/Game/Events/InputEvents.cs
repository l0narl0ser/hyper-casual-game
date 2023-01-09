using System;
using Game.Service;

namespace Game.Events
{
    public class InputEvents
    {
        public event Action<float> OnInputChanged;

        public void ChangeInput(float deltaX)
        {
            OnInputChanged?.Invoke(deltaX);
        }
    }
}