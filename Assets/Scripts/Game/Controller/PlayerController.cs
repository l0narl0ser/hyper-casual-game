using System;
using Core;
using Game.Service;
using UnityEngine;

namespace Game.Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPlatformTriggerable
    {
        [SerializeField] private float _playerSpeed = 5000;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _doudleMainSpriteRender;

        private MessageSystem _messageSystem;
        private BoundService _boundService;

        private void Awake()
        {
            _messageSystem = Context.Instance.GetMessageSystem();
            _boundService = Context.Instance.GetBoundService();
            _messageSystem.InputEvents.OnInputChanged += OnPlayerInputChanged;
        }

        private void FixedUpdate()
        {
            Vector3 playerPosition = transform.position;
            
            if (playerPosition.x > _boundService.RightXPosition)
            {
                transform.position = new Vector3(_boundService.LeftXPosition, playerPosition.y, playerPosition.z);
            }

            if (playerPosition.x < _boundService.LeftXPosition)
            {
                transform.position = new Vector3(_boundService.RightXPosition, playerPosition.y, playerPosition.z);
            }
        }

        private void OnPlayerInputChanged(float deltaX)
        {
            if (deltaX < 0)
            {
                _doudleMainSpriteRender.flipX = true;
            }

            if (deltaX > 0)
            {
                _doudleMainSpriteRender.flipX = false;
            }
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