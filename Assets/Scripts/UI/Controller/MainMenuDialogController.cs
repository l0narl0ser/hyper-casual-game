using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class MainMenuDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField]
        private Button _startGame;

        private MessageSystem _messageSystem;
        private void Awake()
        {
           _startGame.onClick.AddListener(StartGame);
           _messageSystem = Context.Instance.GetMessageSystem();
        }

        private void StartGame()
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