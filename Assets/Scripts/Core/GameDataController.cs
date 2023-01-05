using Game.Data;
using UI.Data;
using UnityEngine;

namespace Core
{
    public class GameDataController
    {
        private GameControllerPrefabData _gameControllerPrefabData;

        public GameControllerPrefabData GameControllerPrefabData => _gameControllerPrefabData;

        private UIDataController _uiDataController;

        public UIDataController UIDataController => _uiDataController;

        public GameDataController()
        {
            _gameControllerPrefabData = Resources.Load<GameControllerPrefabData>("Data/Data_GamePrefabData");
            _uiDataController = Resources.Load<UIDataController>("Data/Data_UIPrefab");
        }
    }
}