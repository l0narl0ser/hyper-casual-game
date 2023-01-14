using System.Collections.Generic;
using Game.Controller;
using UnityEngine;

namespace Game.Service
{
    public class LevelGenerator
    {
        private const int MinBorderRandomY = 3;
        private const int MaxBorderRandomY = 7;
        private const int DistanceFromEdge = 3;
        private const int MaxCountEnemySpawnedInRound = 2;
        private const int CountObjectsToSpawnInRound = 12;
        private const int PercentToSpawnEnemy = 20;
        private const int DistanceBetweenPlatformsInStart = 3;
        private const int CountPlatformsInStart = 4;


        private const int MaxJumpDistance = 14;

        private readonly GameObject _gameWorldRoot;
        private readonly BoundService _boundService;
        private readonly CreateControllerService _createControllerService;
        
        
        private Vector2 _lastGeneratedPosition;
        private Vector2 _lastGeneratedPositionPlatform;
        private Vector2 _lastGeneratedPositionEnemy;


        public LevelGenerator(GameObject gameWorldRoot, BoundService boundService,
            CreateControllerService createControllerService)
        {
            _gameWorldRoot = gameWorldRoot;
            _boundService = boundService;
            _createControllerService = createControllerService;
        }

        public List<IRemovable> GenerateStartSpawn()
        {
            _lastGeneratedPosition = new Vector2(0, -10);
            _lastGeneratedPositionEnemy = _lastGeneratedPosition;
            _lastGeneratedPositionPlatform = _lastGeneratedPosition;
            float xOffset = _boundService.LeftXPosition + DistanceFromEdge;
            List<IRemovable> generatedPlataforms = new List<IRemovable>();
            for (int i = 0; i < CountPlatformsInStart; i++)
            {
                IRemovable removable = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                    _gameWorldRoot.transform,
                    new Vector2(xOffset, -10));
                xOffset += DistanceBetweenPlatformsInStart;
                generatedPlataforms.Add(removable);
            }

            return generatedPlataforms;
        }

        public List<IRemovable> GenerateObjects()
        {
            int roundedLeftPosition = Mathf.RoundToInt(_boundService.LeftXPosition) + DistanceFromEdge;
            int roundedRightPosition = Mathf.RoundToInt(_boundService.RightXPosition) - DistanceFromEdge;
            int countEnemySpawnedInRound = 0;
            List<IRemovable> removables = new List<IRemovable>();

            for (int i = 0; i < CountObjectsToSpawnInRound; i++)
            {
                int deltaYPosition = Random.Range(MinBorderRandomY, MaxBorderRandomY);
                int xPosition = Random.Range(roundedLeftPosition, roundedRightPosition);

               _lastGeneratedPosition =
                    new Vector2(xPosition, _lastGeneratedPosition.y + deltaYPosition);
                IRemovable removable;
                if (CanSpawnEnemy(countEnemySpawnedInRound))
                {
                    removable = _createControllerService.Create<IRemovable>(GameControllerType.SimpleEnemy,
                        _gameWorldRoot.transform, _lastGeneratedPosition);
                    countEnemySpawnedInRound++;
                    _lastGeneratedPositionEnemy = _lastGeneratedPosition;
                    
                }
                else
                {
                    removable = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                        _gameWorldRoot.transform, _lastGeneratedPosition);
                    _lastGeneratedPositionPlatform = _lastGeneratedPosition;
                }

                removables.Add(removable);
            }

            return removables;
        }

        private bool CanSpawnEnemy(int countEnemySpawnedInRound)
        {
            bool needEnemy = Random.Range(0, 100) < PercentToSpawnEnemy;
            if (!needEnemy)
            {
                return false;
            }

            return countEnemySpawnedInRound < MaxCountEnemySpawnedInRound;
        }
    }
}