using Core;
using UnityEngine;

namespace Game.Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPlatformTriggerable
    {
        [SerializeField] private float _playerSpeed = 5000;
        [SerializeField] private Rigidbody2D _rigidbody;

        private MessageSystem _messageSystem;

        private void Awake()
        {
            _messageSystem = Context.Instance.GetMessageSystem();
            _messageSystem.InputEvents.OnInputChanged += OnPlayerInputChanged;
        }

        private void OnPlayerInputChanged(float deltaX)
        {
            _rigidbody.velocity = new Vector2(deltaX * _playerSpeed, _rigidbody.velocity.y);

        }

        public void Trigger(Vector2 pushVector)
        {
            if (_rigidbody.velocity.y > 0)
            {
                return;
            }
            
            _rigidbody.velocity = Vector2.up * pushVector;
        }

        private void OnDestroy()
        {
            _messageSystem.InputEvents.OnInputChanged -= OnPlayerInputChanged;
        }
    }
}