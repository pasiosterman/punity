using UnityEngine;

namespace PUnity.Core.UI
{
    public class BaseMenuView : MonoBehaviour
    {
        private void OnEnable()
        {
            if (!IsVisible)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void ShowView()
        {
            Debug.Log(GetType().Name + " => Show View", this);
            gameObject.SetActive(true);
        }

        public virtual void CloseView()
        {
            Debug.Log(GetType().Name + " => Close View", this);
            gameObject.SetActive(false);
        }

        public virtual void OnUpdateView(float deltaTime)
        {

        }

        bool _isVisible = false;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }
    }
}