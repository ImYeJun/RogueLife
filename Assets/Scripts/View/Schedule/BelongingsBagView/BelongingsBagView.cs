using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.BelongingsBag
{
    public class BelongingsBagView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        private IReadOnlyBelongingsBag belongingsBag;

        private BelongingsSlotView focusedSlot;
        private Belongings focusedBelongings;

        [SerializeField] private GameObject uiRoot;

        [SerializeField] private UnityEvent<Belongings> OnSlotClicked;
        [SerializeField] private List<MainBelongingsSlotView> mainSlots;
        [SerializeField] private GameObject sideSlotPrefab;
        [SerializeField] private Transform sideSlotInventory;
        private IObjectPool<SideBelongingsSlotView> sideSlotPool;
        private List<SideBelongingsSlotView> activeSideSlots = new List<SideBelongingsSlotView>();

        public override void OnInitialized()
        {  
            uiRoot.SetActive(false);
            
            sideSlotPool = new ObjectPool<SideBelongingsSlotView>(
                createFunc : () =>
                {
                    var slot = Instantiate(sideSlotPrefab, sideSlotInventory);
                    slot.SetActive(false);
                    return slot.GetComponent<SideBelongingsSlotView>();
                },
                actionOnGet : (view) =>
                {
                    view.gameObject.SetActive(true);
                },
                actionOnRelease : (view) =>
                {
                    view.Deactive();
                    view.gameObject.SetActive(false);
                },
                actionOnDestroy : (view) =>
                {
                    Destroy(view.gameObject);
                }
            );
            activeSideSlots.Clear();

            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<BelongingsBagChanged>(OnBelongingsBagChanged);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<BelongingsBagChanged>(OnBelongingsBagChanged);
        }

        public void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            belongingsBag = payload.BelongingsBag;

            InitializeView();
        }
        public void OnBelongingsBagChanged(BelongingsBagChanged payload)
        {
            belongingsBag = payload.BelongingsBag;
            DrawView();
        }
        public void OnViewOpened()
        {
            DrawView();
            uiRoot.SetActive(true);
        }

        private void InitializeView()
        {
            focusedSlot = null;
            focusedBelongings = null;

            ClearSlots();
        }

        private void DrawView()
        {
            ClearSlots();
            focusedSlot = null;

            DrawMainBag();
            DrawSideBag();
        }
        private void DrawMainBag()
        {
            var bag = belongingsBag.MainBelongingsBag.Values.ToList();
            for (int i = 0; i < bag.Count; i++)
            {
                var slot = mainSlots[i];
                var belongings = bag[i];

                slot.Activate(belongings, NotifySlotClicked, commander);
                
                if (focusedSlot == null && belongings == focusedBelongings)
                {
                    focusedSlot = slot;
                    focusedSlot.OnFocused();
                }
            }
        }
        private void DrawSideBag()
        {
            foreach (var belongings in belongingsBag.SideBelongingsBag.Values)
            {
                var slot = sideSlotPool.Get();
                slot.Activate(belongings, NotifySlotClicked, commander);
                slot.transform.SetAsLastSibling();

                if (focusedSlot == null && belongings == focusedBelongings)
                {
                    focusedSlot = slot;
                    focusedSlot.OnFocused();
                }

                activeSideSlots.Add(slot);
            }
        }
        public void NotifySlotClicked(BelongingsSlotView slot)
        {
            focusedSlot?.OnUnfocused();

            focusedSlot = slot;
            focusedBelongings = slot.CurrentBelongings;

            focusedSlot.OnFocused();

            OnSlotClicked.Invoke(slot.CurrentBelongings);
        }

        private void ClearSlots()
        {
            foreach (var slot in mainSlots)
            {
                slot.Deactive();
                slot.OnUnfocused();
            }

            foreach (var active in activeSideSlots)
            {
                sideSlotPool.Release(active);
            }
            activeSideSlots.Clear();
        }
    }
}