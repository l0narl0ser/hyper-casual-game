using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Gyroscope = UnityEngine.Gyroscope;

namespace Game.Service
{
    public class InputController : MonoBehaviour
    {
        private MessageSystem _messageSystem;

        private void Awake()
        {
#if UNITY_ANDROID
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            InputSystem.EnableDevice(Accelerometer.current);
            InputSystem.EnableDevice(AttitudeSensor.current);
            InputSystem.EnableDevice(GravitySensor.current);          
#endif
            _messageSystem = Context.Instance.GetMessageSystem();
        }
        
        
    }
}