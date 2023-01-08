using System;
using UnityEngine;

namespace Game.Controller
{
    public class BasePlatformController : MonoBehaviour
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
    }
}