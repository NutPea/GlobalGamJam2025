using UnityEngine;

namespace GetraenkeBub
{
    public interface IUIState 
    {
        void Init();
        void OnBeforeEnter();
        void OnEnter();
        void OnLeave();
    }
}
