using UnityEngine;

namespace Game.Controller
{
    public interface IPlatformTriggerable
    {
        void Trigger(Vector2 pushVector);
    }
}