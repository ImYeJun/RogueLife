using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View.Global;

namespace View.ScheduleView.BelongingsBag
{
    public abstract class BelongingsSlotView : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private GameObject overlap;
        [SerializeField] private Image icon;
        protected Belongings belongings;
        protected IScheduleViewCommander commander;
        protected bool isActivated = false;
        public Action<BelongingsSlotView> OnSlotClicked { get; private set; }
        public Belongings CurrentBelongings => belongings;

        private bool isFocused = false;

        public void Activate(Belongings belongings, Action<BelongingsSlotView> onSlotClicked, IScheduleViewCommander commander)
        {
            OnUnfocused();
            this.belongings = belongings;
            OnSlotClicked = onSlotClicked;
            this.commander = commander;

            icon.sprite = belongings.Image;
            icon.gameObject.SetActive(true);
            isActivated = true;
        }
        public void Deactive()
        {
            icon.gameObject.SetActive(false);

            isActivated = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isActivated) { return; }

            if (isFocused)
            {
                OnFocusedClicked();
            }
            else
            {
                OnSlotClicked.Invoke(this);
            }
        }
        protected abstract void OnFocusedClicked();

        public void OnFocused()
        {
            isFocused = true;
            overlap.SetActive(true);
        }
        public void OnUnfocused()
        {
            isFocused = false;
            overlap.SetActive(false);
        }
    }
}