using UnityEngine;

namespace Game.Controller
{
    public interface IRemovable
    {
        void Remove();

        Vector2 GetPosition();
    }
}