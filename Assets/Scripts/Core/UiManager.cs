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
            _messageSystem.PlayerEvents.OnPlayerDieAnimationFinished += OnPlayerDeadAnimationFinish;
        }

        private void OnPlayerDeadAnimationFinish()
        {
            _dialogControllers[UIDialogType.GameplayDialog].Hide();
            _dialogControllers[UIDialogType.GameOverDialog].Show();
        }

        private void OnStartGame()
        {
            _dialogControllers[UIDialogType.MainDialog].Hide();
            _dialogControllers[UIDialogType.GameOverDialog].Hide();
            _dialogControllers[UIDialogType.GameplayDialog].Show();
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
            _dialogControllers[UIDialogType.MainDialog].Show();
        }

        private void OnDestroy()
        {
            _messageSystem.PlayerEvents.OnStartGame -= OnStartGame;
            _messageSystem.PlayerEvents.OnPlayerDieAnimationFinished -= OnPlayerDeadAnimationFinish;

        }
    }
}