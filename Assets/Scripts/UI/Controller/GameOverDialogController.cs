using System;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Application = Core.Application;

namespace UI.Controller
{
    public class GameOverDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _goToMenu;
        [SerializeField] private Button _restartGame;

        private MessageSystem _messageSystem;

        private void Awake()
        {
            _restartGame.onClick.AddListener(OnRestartButtonClick);
            _goToMenu.onClick.AddListener(OnGoToMenuButtonClick);
            _messageSystem = Context.Instance.GetMessageSystem();
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
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}