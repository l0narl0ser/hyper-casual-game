using Core;
using Game.Controller;
using Game.Data;
using UnityEngine;

namespace Game.Service
{
    public class CreateControllerService
    {
        private readonly GameControllerPrefabData _gameControllerPrefabData;
        public CreateControllerService(GameDataController gameDataController)
        {
            _gameControllerPrefabData = gameDataController.GameControllerPrefabData;
        }
        public GameObject Create(GameControllerType type, Transform parent)
        {
            ControllerModel controllerModelByType = _gameControllerPrefabData.GetControllerModelByType(type);
            GameObject createdObject = GameObject.Instantiate(controllerModelByType.Prefab, parent);
            return createdObject;
        }
    }
}