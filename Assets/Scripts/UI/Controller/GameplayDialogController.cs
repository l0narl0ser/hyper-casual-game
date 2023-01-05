using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controller
{
    public class GameplayDialogController : MonoBehaviour, IDialogController
    {
        [SerializeField] 
        private Button _pauseButton;

        [SerializeField] 
        private TextMeshProUGUI _playerScore;

        private void Awake()
        {
           _pauseButton.onClick.AddListener(OnPauseButtonClick);
           
        }

        private void OnPauseButtonClick()
        {
           
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