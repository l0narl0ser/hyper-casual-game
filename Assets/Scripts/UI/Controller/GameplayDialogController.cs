using System;
using Core;
using Game.Service;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class GameplayDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _pauseButton;

        [SerializeField] private TextMeshProUGUI _playerScore;

        private MessageSystem _messageSystem;
        private ScoreService _scoreService;
        private WorldControlService _worldControlService;
        
        private CompositeDisposable _intervalUpdateScore;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClick);
            _messageSystem = Context.Instance.GetMessageSystem();
            _scoreService = Context.Instance.GetScoreService();
            _worldControlService = Context.Instance.GetWorldCreateService();
        }

        private void OnPauseButtonClick()
        {
            _messageSystem.PlayerEvents.PauseGame();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _intervalUpdateScore?.Dispose();
            _intervalUpdateScore = new CompositeDisposable();

            Observable.Interval(TimeSpan.FromSeconds(0.5f))
                .Where(_ => _worldControlService.WorldExists)
                .Subscribe(_ => UpdatePlayerScore())
                .AddTo(_intervalUpdateScore);
        }

        private void UpdatePlayerScore()
        {
            _playerScore.text = _scoreService.PlayerScore.ToString();
        }

        public void Hide()
        {
            _intervalUpdateScore?.Dispose();
            gameObject.SetActive(false);
        }
    }
}