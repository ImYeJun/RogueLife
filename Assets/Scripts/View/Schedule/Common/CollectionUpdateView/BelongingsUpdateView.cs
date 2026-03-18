using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.CollectionUpdateView
{
    public class BelongingsUpdateView : MonoBehaviour
    {
        [SerializeField] private GameObject updateViewObject;
        [SerializeField] private BelongingsView belongingsView;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private Color obtainedColor = Color.green;
        private Action OnUpdateConfirmed;

        private void Awake() {
            updateViewObject.SetActive(false);
            belongingsView.OnClicked = OnBelongingsClicked;
        }

        public void OnObtained(BelongingsObtained payload, Action onUpdateConfirmed)
        {
            header.text = "소지품 획득";
            header.color = obtainedColor;
            
            updateViewObject.SetActive(true);
            OnUpdateConfirmed = onUpdateConfirmed;
            belongingsView.SetBelongings(payload.Belongings);
        }

        public void OnBelongingsClicked()
        {
            updateViewObject.SetActive(false);
            OnUpdateConfirmed?.Invoke();
        }

        public void SetActive(bool value)
        {
            updateViewObject.SetActive(value);
        }
    }
}