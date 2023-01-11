using Core;
using Game.Service;
using UnityEngine;

namespace Game.Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPlatformTriggerable, IRemovable
    {
        [SerializeField] private float _playerSpeed = 5000;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _doudleMainSpriteRender;

        private MessageSystem _messageSystem;
        private BoundService _boundService;
        private CreateControllerService _createControllerService;
        private bool _playerDead;

        private void Awake()
        {
            _messageSystem = Context.Instance.GetMessageSystem();
            _boundService = Context.Instance.GetBoundService();
            _createControllerService = Context.Instance.GetCreateControlService();
            _messageSystem.InputEvents.OnInputAccelerationChanged += OnPlayerInputAccelerationChanged;
            _messageSystem.InputEvents.OnTouched += OnTouched;
        }

        private void OnTouched(Vector2 touchPosition)
        {
            _createControllerService.Create<BulletController>(GameControllerType.Bullet, transform.parent,
                transform.position);
            
        }

        private void FixedUpdate()
        {
            if (_playerDead)
            {
                return;
            }
            Vector3 playerPosition = transform.position;
            
            CheckXPlayerPosition(playerPosition);
            CheckIsPlayerDead(playerPosition);
        }

        private void CheckIsPlayerDead(Vector3 playerPosition)
        {
            if (!_boundService.IsYDownCamera(playerPosition.y))
            {
                return;
            }
            
            _playerDead = true;
            _messageSystem.PlayerEvents.PlayerDead();
        }

        private void CheckXPlayerPosition(Vector3 playerPosition)
        {
            if (playerPosition.x > _boundService.RightXPosition)
            {
                transform.position = new Vector3(_boundService.LeftXPosition, playerPosition.y, playerPosition.z);
            }

            if (playerPosition.x < _boundService.LeftXPosition)
            {
                transform.position = new Vector3(_boundService.RightXPosition, playerPosition.y, playerPosition.z);
            }
        }

        private void OnPlayerInputAccelerationChanged(float deltaX)
        {
            if (_playerDead)
            {
                return;
            }
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
            _messageSystem.InputEvents.OnInputAccelerationChanged -= OnPlayerInputAccelerationChanged;
        }

        public bool PlayerDead => _playerDead;
        public void Remove()
        {
            Destroy(gameObject);
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }
}