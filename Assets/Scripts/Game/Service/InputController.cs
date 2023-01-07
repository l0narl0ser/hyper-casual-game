﻿using System;
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
#if !UNITY_EDITOR
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            InputSystem.EnableDevice(Accelerometer.current);
            InputSystem.EnableDevice(AttitudeSensor.current);
            InputSystem.EnableDevice(GravitySensor.current);          
#endif
            _messageSystem = Context.Instance.GetMessageSystem();
        }

        private void Update()
        {
#if !UNITY_EDITOR
            var gyroscope = UnityEngine.InputSystem.Gyroscope.current;
            Vector3 angularVelocity = gyroscope.angularVelocity.ReadValue();
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            var inputModel = new InputModel(acceleration, angularVelocity);
            //TODO: Какие-то преобразования 
            _messageSystem.InputEvents.ChangeInput(inputModel);
            Debug.Log(inputModel);
#endif

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                
            }
#endif
        }
    }
}