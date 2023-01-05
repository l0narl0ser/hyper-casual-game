

namespace Core
{
    public class Context
    {
        private static Context _instance;

        private readonly MessageSystem _messageSystem;
        private readonly SnapshotManager _snapshotManager;
        private readonly GameDataController _gameDataController;

        public Context()
        {
            _messageSystem = new MessageSystem();
            _snapshotManager = new SnapshotManager();
            _gameDataController = new GameDataController();
        }

        public static Context Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new Context();
                return _instance;
            }
        }

        public MessageSystem GetMessageSystem()
        {
            return _messageSystem;
        }

        public SnapshotManager GetSnapshotManager()
        {
            return _snapshotManager;
        }

        public GameDataController GetGameDataController()
        {
            return _gameDataController;
        }
    }
}