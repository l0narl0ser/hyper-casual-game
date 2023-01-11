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
        private bool touchStarted;

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
            DetectMoving();
            DetectTap();
        }

        private void DetectTap()
        {
            
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetMouseButtonDown(0))
            {
                _messageSystem.InputEvents.Touch(Input.mousePosition);
            }

#else
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved )
                {
                    return;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    touchStarted = false;
                }

                if (touch.phase == TouchPhase.Began && touchStarted)
                {
                   return;
                }
                
                if (touch.phase == TouchPhase.Began)
                {
                    touchStarted = true;                   

                    _messageSystem.InputEvents.Touch(touch.position);

                }
            }
#endif
        }

        private void DetectMoving()
        {
#if !UNITY_EDITOR
            var gyroscope = UnityEngine.InputSystem.Gyroscope.current;
            Vector3 angularVelocity = gyroscope.angularVelocity.ReadValue();
            Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
            
            _messageSystem.InputEvents.ChangeInput(acceleration.x * Time.deltaTime);
#endif

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _messageSystem.InputEvents.ChangeAcceleration(-1 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _messageSystem.InputEvents.ChangeAcceleration(1 * Time.deltaTime);
            }
#endif
        }
    }
}