using Core;
using Game.Service;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Game.Controller
{
    public class BulletController : MonoBehaviour, IRemovable
    {
        [SerializeField] private Vector3 _offsetBullet;
        [SerializeField] private float _bulletSpeed = 3;

        private BoundService _boundService;

        private void Awake()
        {
            _boundService = Context.Instance.GetBoundService();
        }

        private void FixedUpdate()
        {
            if (_boundService.IsYUpCamera(transform.position.y))
            {
                Remove();
                return;
            }

            var newPosition = transform.position + _offsetBullet;
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _bulletSpeed);
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        public Vector2 GetPosition()
        {
            return gameObject.transform.position;
        }
    }
}