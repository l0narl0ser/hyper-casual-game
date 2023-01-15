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
        private const int CountPlatformsToSpawnInRound = 10;
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
                generatedPlataforms.Add(GeneratePlatform(roundedLeftPosition, roundedRightPosition));
            }

            return generatedPlataforms;
        }

        public List<IRemovable> GenerateObjects()
        {
            int roundedLeftPosition = Mathf.RoundToInt(_boundService.LeftXPosition) + DistanceFromEdge;
            int roundedRightPosition = Mathf.RoundToInt(_boundService.RightXPosition) - DistanceFromEdge;
            List<IRemovable> removables = new List<IRemovable>();

            for (int i = 0; i < CountPlatformsToSpawnInRound; i++)
            {
                removables.Add(GeneratePlatform(roundedLeftPosition, roundedRightPosition));
            }

            removables.Add(GenerateSpring(roundedLeftPosition, roundedRightPosition));
            
            int countEnemySpawnedInRound = 0;

            for (int i = 0; i < removables.Count - 1; i++)
            {
                if (!CanSpawnEnemy(countEnemySpawnedInRound, removables[i], removables[i + 1]))
                {
                    continue;
                }

                countEnemySpawnedInRound++;
                var removable = GenerateEnemy(removables[i]);
                removables.Add(removable);
            }

            return removables;
        }

        private IRemovable GenerateEnemy(IRemovable sourceRemovable)
        {
            GameControllerType typeEnemyToSpawn = Random.Range(0, 100) > 50
                ? GameControllerType.SimpleEnemy
                : GameControllerType.StayEnemy;
            IRemovable removable = _createControllerService.Create<IRemovable>(typeEnemyToSpawn,
                _gameWorldRoot.transform, sourceRemovable.GetPosition() + new Vector2(0, EnemyYOffsetFromPlatform));
            return removable;
        }

        private IRemovable GeneratePlatform(int roundedLeftPosition, int roundedRightPosition)
        {
            int deltaYPosition = Random.Range(MinBorderRandomY, MaxBorderRandomY);
            int xPosition = Random.Range(roundedLeftPosition, roundedRightPosition);
            _lastGeneratedPosition =
                new Vector2(xPosition, _lastGeneratedPosition.y + deltaYPosition);


            IRemovable removable = _createControllerService.Create<IRemovable>(GameControllerType.StandardPlatform,
                _gameWorldRoot.transform, _lastGeneratedPosition);
            return removable;
        }

        private IRemovable GenerateSpring(int roundedLeftPosition, int roundedRightPosition)
        {
            int deltaYSpringPlatform = Random.Range(MinBorderRandomY, MaxBorderRandomY);
            int xSpringPlatform = Random.Range(roundedLeftPosition, roundedRightPosition);
            _lastGeneratedPosition =
                new Vector2(deltaYSpringPlatform, _lastGeneratedPosition.y + xSpringPlatform);

            IRemovable removableSpring = _createControllerService.Create<IRemovable>(
                GameControllerType.SpringPlatform,
                _gameWorldRoot.transform, _lastGeneratedPosition);
            return removableSpring;
        }

        private bool CanSpawnEnemy(int countEnemySpawnedInRound, IRemovable firstPlatform, IRemovable secondPlatform)
        {
            float distanceByYBetweenPlatforms = secondPlatform.GetPosition().y - firstPlatform.GetPosition().y;
            return countEnemySpawnedInRound < MaxCountEnemySpawnedInRound &&
                   distanceByYBetweenPlatforms > RightDistanceToSpawnEnemy;
        }

        public bool CanGenerateObjectsInNewRound(float playerYPosition)
        {
            return _lastGeneratedPosition.y - playerYPosition < 30;
        }
    }
}