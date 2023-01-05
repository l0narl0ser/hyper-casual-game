using System;
using Game.Controller;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class ControllerModel
    {
        public GameControllerType GameControllerType;
        public GameObject Prefab;
    }
}