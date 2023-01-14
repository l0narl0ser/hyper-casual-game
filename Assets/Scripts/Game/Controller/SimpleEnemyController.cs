using Core;
using Game.Service;
using UnityEngine;

namespace Game.Controller
{
    public class SimpleEnemyController : MonoBehaviour, IBulletTriggerable, IRemovable
    {
        private const int ChangeDirectionCoef = -1;
        [SerializeField] private int _deltaXBeforeChangeDirection;
        [SerializeField] private float _enemyMovingSpeed;

        private Vector3 _directionMoving = new Vector3(1, 0, 0);
        private BoundService _boundService;

        private void Awake()
        {
            _boundService = Context.Instance.GetBoundService();
        }

        private void FixedUpdate()
        {
            if (_boundService.LeftXPosition + _deltaXBeforeChangeDirection > transform.position.x)
            {
                _directionMoving *= ChangeDirectionCoef;
            }
            
            if (_boundService.RightXPosition - _deltaXBeforeChangeDirection < transform.position.x)
            {
                _directionMoving *= ChangeDirectionCoef;
            }

            Vector3 newPosition = transform.position + _directionMoving;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _enemyMovingSpeed);
        }

        public void TriggerOnBullet()
        {
            Remove();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            IEnemyDyeable enemyDyeable = col.GetComponent<IEnemyDyeable>();
            if (enemyDyeable == null)
            {
                return;
            }

            enemyDyeable.Die();
        }

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