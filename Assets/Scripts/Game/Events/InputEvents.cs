using System;
using Game.Service;
using UnityEngine;

namespace Game.Events
{
    public class InputEvents
    {
        public event Action<float> OnInputAccelerationChanged;
        public event Action<Vector2> OnTouched;

        public void Touch(Vector2 touchPosition)
        {
            OnTouched?.Invoke(touchPosition);
        }

        public void ChangeAcceleration(float deltaX)
        {
            OnInputAccelerationChanged?.Invoke(deltaX);
        }
    }
}