using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "Data_GamePrefabData", menuName = "Data/Controller/Game Prefab")]
    public class GameControllerPrefabData : ScriptableObject
    {
        [SerializeField] private List<ControllerModel> _controllerModels = new List<ControllerModel>();
    }
}