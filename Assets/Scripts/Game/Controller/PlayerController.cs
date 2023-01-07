using UnityEngine;

namespace Game.Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IPlatformTriggerable
    {
        [SerializeField] private Rigidbody2D _rigidbody;
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
    }
}