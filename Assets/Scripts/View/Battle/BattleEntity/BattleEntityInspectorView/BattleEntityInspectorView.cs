using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattleEntityInspectorView : MonoBehaviour, IInspectorBuilder
    {
        public enum InspectorDirection
        {
            Left, Right
        }

        private struct RectState
        {
            public Vector2 anchorMin;
            public Vector2 anchorMax;
            public Vector2 pivot;
            public Vector2 anchoredPosition;

            public RectState(RectTransform rect)
            {
                anchorMin = rect.anchorMin;
                anchorMax = rect.anchorMax;
                pivot = rect.pivot;
                anchoredPosition = rect.anchoredPosition;
            }

            public void Apply(RectTransform rect, bool isMirrored)
            {
                if (isMirrored)
                {
                    rect.anchorMin = new Vector2(1f - anchorMax.x, anchorMin.y);
                    rect.anchorMax = new Vector2(1f - anchorMin.x, anchorMax.y);
                    rect.pivot = new Vector2(1f - pivot.x, pivot.y);
                    rect.anchoredPosition = new Vector2(-anchoredPosition.x, anchoredPosition.y);
                }
                else
                {
                    rect.anchorMin = anchorMin;
                    rect.anchorMax = anchorMax;
                    rect.pivot = pivot;
                    rect.anchoredPosition = anchoredPosition;
                }
            }
        }

        [SerializeField] private RectTransform panelTransform;
        [SerializeField] private RectTransform closeButtonTransform; 
        [SerializeField] private RectTransform panelContentTransform;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject normalTextPrefab;
        [SerializeField] private GameObject mainTextPrefab;
        [SerializeField] private GameObject nameTextPrefab;
        [SerializeField] private GameObject subPanelPrefab;

        private RectState defaultPanelState;
        private RectState defaultCloseState;
        private bool isInitialized = false;

        private void Awake()
        {
            InitStates();
        }

        private void InitStates()
        {
            if (isInitialized) return;
            
            defaultPanelState = new RectState(panelTransform);
            defaultCloseState = new RectState(closeButtonTransform);
            isInitialized = true;
        }

        public void InspectEntity(IInspectable inspectable, InspectorDirection direction)
        {
            InitStates(); 

            bool isMirrored = direction == InspectorDirection.Left;
            
            defaultPanelState.Apply(panelTransform, isMirrored);
            defaultCloseState.Apply(closeButtonTransform, isMirrored);

            ClearPanel(); 
            
            inspectable.OnInspect(this, panelContentTransform);
        }

        private void ClearPanel()
        {
            for (int i = panelContentTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(panelContentTransform.GetChild(i).gameObject);
            }
        }

        public InspectorNormalText AddNormalText(RectTransform parent)
        {
            var gameObj = Instantiate(normalTextPrefab, parent);
            return gameObj.GetComponent<InspectorNormalText>();
        }

        public InspectorMainText AddMainText(RectTransform parent)
        {
            var gameObj = Instantiate(mainTextPrefab, parent);
            return gameObj.GetComponent<InspectorMainText>();
        }

        public InspectorNameText AddNameText(RectTransform parent)
        {
            var gameObj = Instantiate(nameTextPrefab, parent);
            return gameObj.GetComponent<InspectorNameText>();
        }

        public InspectorSubPanel AddSubPanel(RectTransform parent)
        {
            var gameObj = Instantiate(subPanelPrefab, parent);
            return gameObj.GetComponent<InspectorSubPanel>();
        }
    }
}