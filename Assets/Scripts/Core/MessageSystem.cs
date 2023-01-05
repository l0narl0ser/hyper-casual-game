using Game.Events;

namespace Core
{
    public class MessageSystem
    {
        public readonly PlayerEvents PlayerEvents = new PlayerEvents();
        public readonly InputEvents InputEvents = new InputEvents();
    }
}