using System;
using Game.Service;

namespace Game.Events
{
    public class InputEvents
    {
        public event Action<InputModel> OnInputChanged;

        public void ChangeInput(InputModel inputModel)
        {
            OnInputChanged?.Invoke(inputModel);
        }
    }
}