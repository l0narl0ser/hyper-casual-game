using System;
using Core;
using UnityEngine;

namespace Game.Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPlatformTriggerable
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private MessageSystem _messageSystem;

        private void Awake()
        {
            _messageSystem = Context.Instance.GetMessageSystem();
            _messageSystem.InputEvents.OnInputChanged += OnPlayerInputChanged;
        }

        private void OnPlayerInputChanged(float deltaX)
        {
            Debug.Log($"DEltax = {deltaX}");
            var newPosition = transform.position;
            newPosition +=
                new Vector3(newPosition.x + deltaX, newPosition.y, newPosition.z);
        }

        public void Trigger(Vector2 pushVector)
        {
            if (_rigidbody.velocity.y > 0)
            {
                return;
            }

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0;
            _rigidbody.angularDrag = 0;
            _rigidbody.AddForce(pushVector);
        }

        private void OnDestroy()
        {
            _messageSystem.InputEvents.OnInputChanged -= OnPlayerInputChanged;
        }
    }
}