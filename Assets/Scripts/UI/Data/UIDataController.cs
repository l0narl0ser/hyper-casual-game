using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Data
{
    [CreateAssetMenu(fileName = "Data_UIPrefab", menuName = "Data/UI/UI Prefab")]
    public class UIDataController : ScriptableObject
    {
        [SerializeField] private List<UiDialogModel> _uiDialogModels = new List<UiDialogModel>();

        public UiDialogModel GetDialogModel(UIDialogType uiDialogType)
        {
            UiDialogModel firstOrDefault = _uiDialogModels
                .FirstOrDefault(dialog => dialog.UIDialogType == uiDialogType);
            
            /*for (int i = 0; i < _uiDialogModels.Count; i++)
            {
                if (_uiDialogModels[i].UIDialogType == uiDialogType)
                {
                    firstOrDefault = _uiDialogModels[i];
                } 
            }*/
            if (firstOrDefault == null)
            {
                Debug.LogError($"Dialog of type {uiDialogType} not registreated");
            }

            return firstOrDefault;
        }

        public List<UiDialogModel> UIDialogModels => _uiDialogModels;
    }
}