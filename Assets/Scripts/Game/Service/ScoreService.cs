using System;
using Core;
using UniRx;
using UnityEngine;
using Observable = UniRx.Observable;

namespace Game.Service
{
    public class ScoreService : IDisposable
    {
        private const float ScoreIncreaseCoefficient = 2.5f;
        private readonly WorldControlService _worldControlService;
        private readonly MessageSystem _messageSystem;
        private readonly SnapshotManager _snapshotManager;
        private CompositeDisposable _compositeDisposable;

        private int _playerScore;

        public ScoreService(WorldControlService worldControlService, MessageSystem messageSystem,
            SnapshotManager snapshotManager)
        {
            _worldControlService = worldControlService;
            _messageSystem = messageSystem;
            _snapshotManager = snapshotManager;
            _messageSystem.PlayerEvents.OnStartGame += OnStartGame;
            _messageSystem.PlayerEvents.OnPlayerDead += OnPlayerDead;
        }

        private void OnPlayerDead()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
            int lastMaxScore = _snapshotManager.GetScore();

            if (lastMaxScore > _playerScore)
            {
                return;
            }

            _snapshotManager.SetScore(_playerScore);
            _snapshotManager.Save();
        }

        private void OnStartGame()
        {
            _playerScore = 0;
            _compositeDisposable?.Dispose();
            _compositeDisposable = new CompositeDisposable();
            Observable.Interval(TimeSpan.FromSeconds(0.5f))
                .Where(_ => _worldControlService.WorldExists)
                .Subscribe(_ => UpdateScore())
                .AddTo(_compositeDisposable);
        }

        private void UpdateScore()
        {
            if (_worldControlService.PlayerController.transform.position.y * ScoreIncreaseCoefficient <= _playerScore)
            {
                return;
            }

            _playerScore = Mathf.RoundToInt(_worldControlService.PlayerController.transform.position.y *
                                            ScoreIncreaseCoefficient);
        }

        public void Dispose()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnStartGame;
            _messageSystem.PlayerEvents.OnPlayerDead -= OnPlayerDead;
            _compositeDisposable?.Dispose();
        }

        public int PlayerScore => _playerScore;
    }
}