using System.Collections.Generic;
using UnityEngine;

namespace PUnity.Core.UI
{
    public class MenuViewController : MonoBehaviour
    {
        private void Update()
        {
            if (CurrentMenuView != null && CurrentMenuView.IsVisible)
            {
                float dt = Time.deltaTime;
                CurrentMenuView.OnUpdateView(dt);
            }
        }

        public void ShowMenuView(BaseMenuView menuView, bool clearTransitionHistory = false)
        {
            if (clearTransitionHistory)
                TransitionHistory.Clear();

            CurrentMenuView = menuView;
            CurrentMenuView.ShowView();
        }

        public void TransitionToMenuView(BaseMenuView menuView)
        {
            if (CurrentMenuView != null)
                AddToTransitionHistory(CurrentMenuView);

            CurrentMenuView = menuView;
            CurrentMenuView.ShowView();
        }

        void AddToTransitionHistory(BaseMenuView menuView)
        {
            if (!TransitionHistory.Contains(menuView))
            {
                TransitionHistory.Add(menuView);
            }
            else
            {
                Debug.LogError(GetType().Name + " MenuView already in transition history, flushing all views from history after given view", this);

                int index = TransitionHistory.IndexOf(menuView);
                for (int i = TransitionHistory.Count - 1; i > index; i--)
                    TransitionHistory.RemoveAt(i);
            }
        }

        private BaseMenuView _currentMenuView = null;
        public BaseMenuView CurrentMenuView
        {
            get { return _currentMenuView; }
            private set { _currentMenuView = value; }
        }

        private List<BaseMenuView> _transitionHistory;
        /// <summary>
        /// Used to determine which view to transition to (if any) when previous view  
        /// </summary>
        public List<BaseMenuView> TransitionHistory
        {
            get { return _transitionHistory; }
            private set { _transitionHistory = value; }
        }
    } 
}