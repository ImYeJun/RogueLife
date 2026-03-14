using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattleEntityInspectorView : MonoBehaviour, IInspectorBuilder
    {
        [SerializeField] private RectTransform panelTransform;
        [SerializeField] private GameObject normalTextPrefab;
        [SerializeField] private GameObject mainTextPrefab;
        [SerializeField] private GameObject nameTextPrefab;
        [SerializeField] private GameObject subPanelPrefab;

        public void InspectEntity(IInspectable inspectable)
        {
            ClearPanel(); 
            
            inspectable.OnInspect(this, panelTransform);
        }

        private void ClearPanel()
        {
            for (int i = panelTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(panelTransform.GetChild(i).gameObject);
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