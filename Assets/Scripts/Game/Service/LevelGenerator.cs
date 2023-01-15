using System.Collections.Generic;
using Game.Controller;
using UnityEngine;

namespace Game.Service
{
    public class LevelGenerator
    {
        private const int MinBorderRandomY = 3;
        private const int MaxBorderRandomY = 9;
        private const int DistanceFromEdge = 3;
        private const int MaxCountEnemySpawnedInRound = 2;
        private const int CountPlatformsToSpawnInRound = 12;
        private const int DistanceBetweenPlatformsInStart = 3;
        private const int CountPlatformsInStart = 4;
        private const int RightDistanceToSpawnEnemy = 6;
        private const int EnemyYOffsetFromPlatform = 3;


        private readonly GameObject _gameWorldRoot;
        private readonly BoundService _boundService;
        private readonly CreateControllerService _createControllerService;


        private Vector2 _lastGeneratedPosition;

        public LevelGenerator(GameObject gameWorldRoot, BoundService boundService,
            CreateControllerService createControllerService)
        {
            _gameWorldRoot = gameWorldRoot;
            _boundService = boundService;
            _createControllerService = createControllerService;
        }

        public List<IRemovable> GenerateStartSpawn()
        {
            int roundedLeftPosition = Mathf.RoundToInt(_boundService.LeftXPosition) + DistanceFromEdge;
            int roundedRightPosition = Mathf.RoundToInt(_boundService.RightXPosition) - DistanceFromEdge;
            _lastGeneratedPosition = new Vector2(0, -10);
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
            
            for (int i = 0; i < CountPlatformsToSpawnInRound; i++)
            {
                int deltaYPosition = Random.Range(MinBorderRandomY, MaxBorderRandomY);
                int xPosition = Random.Range(roundedLeftPosition, roundedRightPosition);
                _lastGeneratedPosition =
                    new Vector2(xPosition, _lastGeneratedPosition.y + deltaYPosition);


                IRemovable removable = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                    _gameWorldRoot.transform, _lastGeneratedPosition);
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

            for (int i = 0; i < CountPlatformsToSpawnInRound; i++)
            {
                int deltaYPosition = Random.Range(MinBorderRandomY, MaxBorderRandomY);
                int xPosition = Random.Range(roundedLeftPosition, roundedRightPosition);
                _lastGeneratedPosition =
                    new Vector2(xPosition, _lastGeneratedPosition.y + deltaYPosition);


                IRemovable removable = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                    _gameWorldRoot.transform, _lastGeneratedPosition);
                removables.Add(removable);
            }

            for (int i = 0; i < removables.Count - 1; i++)
            {
                if (!CanSpawnEnemy(countEnemySpawnedInRound, removables[i], removables[i + 1]))
                {
                    continue;
                }

                countEnemySpawnedInRound++;
                IRemovable removable = _createControllerService.Create<IRemovable>(GameControllerType.SimpleEnemy,
                    _gameWorldRoot.transform, removables[i].GetPosition() + new Vector2(0, EnemyYOffsetFromPlatform));
                removables.Add(removable);
            }

            return removables;
        }

        private bool CanSpawnEnemy(int countEnemySpawnedInRound, IRemovable firstPlatform, IRemovable secondPlatform)
        {
            float distanceByYBetweenPlatforms = secondPlatform.GetPosition().y - firstPlatform.GetPosition().y;
            return countEnemySpawnedInRound <= MaxCountEnemySpawnedInRound &&
                   distanceByYBetweenPlatforms > RightDistanceToSpawnEnemy;
        }
    }
}