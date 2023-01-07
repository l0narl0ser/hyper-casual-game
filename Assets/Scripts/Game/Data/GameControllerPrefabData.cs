using System;
using System.Collections.Generic;
using Game.Controller;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "Data_GamePrefabData", menuName = "Data/Controller/Game Prefab")]
    public class GameControllerPrefabData : ScriptableObject
    {
        [SerializeField] private List<ControllerModel> _controllerModels = new List<ControllerModel>();

        public ControllerModel GetControllerModelByType(GameControllerType gameControllerType)
        {
            foreach (var controllerModel in _controllerModels)
            {
                if (controllerModel.GameControllerType == gameControllerType)
                {
                    return controllerModel;
                }
            }
            Debug.LogError($"Not Found Prefab for controller type {gameControllerType}");
            throw new ArgumentException();
        }
    }
}