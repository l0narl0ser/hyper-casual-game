using UnityEngine;

namespace Game.Controller
{
    public class BasePlatformController : MonoBehaviour, IRemovable
    {
        [SerializeField] private float _pushUpForce = 15;

        private void OnTriggerEnter2D(Collider2D other)
        {
            IPlatformTriggerable platformTriggerable = other.transform.gameObject.GetComponent<IPlatformTriggerable>();
            if (platformTriggerable != null)
            {
                platformTriggerable.Trigger(new Vector2(0, _pushUpForce));
            }
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