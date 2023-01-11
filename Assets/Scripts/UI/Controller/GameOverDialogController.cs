using System;
using Core;
using Game.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Application = Core.Application;

namespace UI.Controller
{
    public class GameOverDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _goToMenu;
        [SerializeField] private Button _restartGame;
        [SerializeField] private TextMeshProUGUI _highestScore;
        [SerializeField] private TextMeshProUGUI _currentScore;

        private MessageSystem _messageSystem;
        private ScoreService _scoreService;
        private SnapshotManager _snapshotManager;

        private void Awake()
        {
            _restartGame.onClick.AddListener(OnRestartButtonClick);
            _goToMenu.onClick.AddListener(OnGoToMenuButtonClick);
            _messageSystem = Context.Instance.GetMessageSystem();
            _scoreService = Context.Instance.GetScoreService();
            _snapshotManager = Context.Instance.GetSnapshotManager();
        }

        private void OnGoToMenuButtonClick()
        {
            Application.Instance.Restart();
        }

        private void OnRestartButtonClick()
        {
            _messageSystem.PlayerEvents.StartGame(); 
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateView();
        }

        private void UpdateView()
        {
            _highestScore.text = _snapshotManager.GetScore().ToString();
            _currentScore.text = _scoreService.PlayerScore.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}