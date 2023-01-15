using System;
using UnityEngine;

namespace Game.Controller
{
    public class SpringPlatformController : MonoBehaviour, IRemovable
    {
        [SerializeField] private float _pushUpByPlatform = 25;
        [SerializeField] private float _pushUpBySpring = 150;
        [SerializeField] private Collider2D _platformCollider;


        public void Remove()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IPlatformTriggerable platformTriggerable = other.transform.gameObject.GetComponent<IPlatformTriggerable>();
            if (platformTriggerable == null)
            {
                return;
            }

            if (other.IsTouching(_platformCollider))
            {
                platformTriggerable.Trigger(new Vector2(0, _pushUpByPlatform));
            }
            else
            {
                platformTriggerable.Trigger(new Vector2(0, _pushUpBySpring));
            }
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
    }
}