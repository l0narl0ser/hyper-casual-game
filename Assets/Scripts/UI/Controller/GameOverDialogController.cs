using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class GameOverDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _goToMenu;
        [SerializeField] private Button _restartGame;

        private void Awake()
        {
            _restartGame.onClick.AddListener(OnRestartButtonClick);
            _goToMenu.onClick.AddListener(OnGoToMenuButtonClick);
        }

        private void OnGoToMenuButtonClick()
        {
            throw new NotImplementedException();
        }

        private void OnRestartButtonClick()
        {
            throw new NotImplementedException();
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