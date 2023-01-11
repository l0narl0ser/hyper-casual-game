﻿using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class GameplayDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _pauseButton;

        [SerializeField] private TextMeshProUGUI _playerScore;

        private MessageSystem _messageSystem;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClick);
            _messageSystem = Context.Instance.GetMessageSystem();
        }

        private void OnPauseButtonClick()
        {
            _messageSystem.PlayerEvents.PauseGame();
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