

using Game.Service;

namespace Core
{
    public class Context
    {
        private static Context _instance;

        private readonly MessageSystem _messageSystem;
        private readonly SnapshotManager _snapshotManager;
        private readonly GameDataController _gameDataController;
        private readonly WorldControlService _worldControlService;
        private readonly CreateControllerService _createControllerService;
        private readonly PlayerCameraService _playerCameraService;
        private readonly BoundService _boundService;
        private readonly PauseService _pauseService;

        private Context()
        {
            _messageSystem = new MessageSystem();
            _snapshotManager = new SnapshotManager();
            _gameDataController = new GameDataController();
            
            _createControllerService = new CreateControllerService(_gameDataController);
            _boundService = new BoundService(_messageSystem);


            _worldControlService = new WorldControlService(_messageSystem, _createControllerService, _boundService);
            
            _playerCameraService = new PlayerCameraService(_worldControlService, _messageSystem);

            _pauseService = new PauseService(_messageSystem);
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

        public void ClearContext()
        {
            _instance = null;
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

        public WorldControlService GetWorldCreateService()
        {
            return _worldControlService;
        }

        public CreateControllerService GetCreateControlService()
        {
            return _createControllerService;
        }

        public BoundService GetBoundService()
        {
            return _boundService;
        }
    }
}