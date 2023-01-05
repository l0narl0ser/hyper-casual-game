using System;
using System.Collections.Generic;
using UI.Data;
using UnityEngine;

namespace Core
{
    public class UiManager : MonoBehaviour
    {
        private MessageSystem _messageSystem;
        private GameDataController _gameDataController;

        private Dictionary<UIDialogType, IDialogController> _dialogControllers =
            new Dictionary<UIDialogType, IDialogController>();

        private void Awake()
        {
            _messageSystem = Context.Instance.GetMessageSystem();
            _gameDataController = Context.Instance.GetGameDataController();
            _messageSystem.PlayerEvents.OnStartGame += OnStartGame;
        }

        private void OnStartGame()
        {
            _dialogControllers[UIDialogType.MainMenu].Hide();
            _dialogControllers[UIDialogType.GameplayMenu].Show();
        }

        private void Start()
        {
            CreateAllDialogs();
            ConfigureStartView();
        }

        private void CreateAllDialogs()
        {
            foreach (UiDialogModel uiDialogModel in _gameDataController.UIDataController.UIDialogModels)
            {
                GameObject instantiate = Instantiate(uiDialogModel.UIPrefab, gameObject.transform);
                
                IDialogController dialogController = instantiate.GetComponent<IDialogController>();
                _dialogControllers.Add(uiDialogModel.UIDialogType, dialogController);
                dialogController.Hide();
            }
        }

        private void ConfigureStartView()
        {
            _dialogControllers[UIDialogType.MainMenu].Show();
        }

        private void OnDestroy()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnStartGame;
        }
    }
}