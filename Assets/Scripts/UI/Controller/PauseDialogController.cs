using System;
using Core;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Controller
{
    public class PauseDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] private Button _resumeButton;
        private MessageSystem _messageSystem;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(OnResumeGame);
            _messageSystem = Context.Instance.GetMessageSystem();
        }

        private void OnResumeGame()
        {
            _messageSystem.PlayerEvents.UnpauseGame();
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