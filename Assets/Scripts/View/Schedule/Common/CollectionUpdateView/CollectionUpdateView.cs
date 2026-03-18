using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.CollectionUpdateView
{
    public class CollectionUpdateView : ViewBehaviour<IScheduleViewEvent>
    {
        public const int CollectionUpdate_Priority = 10;

        [SerializeField] private GameObject panelView; 
        private CardUpdateView cardUpdateView;
        private BelongingsUpdateView belongingsUpdateView;

        private enum UpdateType { CardObtained, CardRemoved, BelongingObtained }
        private struct UpdateData
        {
            public UpdateType Type;
            public object Payload;
        }

        private Queue<UpdateData> updateQueue = new Queue<UpdateData>();
        
        private bool isProcessing = false;
        private bool isUserConfirmed = false;

        private void Awake() {
            panelView.SetActive(false);
            cardUpdateView = GetComponent<CardUpdateView>();
            belongingsUpdateView = GetComponent<BelongingsUpdateView>();
        }

        public override void OnInitialized()
        {
            eventBus.Subscribe<CardObtained>(OnCardObtained);
            eventBus.Subscribe<CardRemoved>(OnCardRemoved);
            eventBus.Subscribe<BelongingsObtained>(OnBelongingsObtained);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<CardObtained>(OnCardObtained);
            eventBus?.Unsubscribe<CardRemoved>(OnCardRemoved);
            eventBus?.Unsubscribe<BelongingsObtained>(OnBelongingsObtained);
        }

        public void OnCardObtained(CardObtained payload)
        {
            updateQueue.Enqueue(new UpdateData { Type = UpdateType.CardObtained, Payload = payload });
            TryStartProcessing(payload.SequenceId);
        }

        public void OnCardRemoved(CardRemoved payload)
        {
            updateQueue.Enqueue(new UpdateData { Type = UpdateType.CardRemoved, Payload = payload });
            TryStartProcessing(payload.SequenceId);
        }

        public void OnBelongingsObtained(BelongingsObtained payload)
        {
            updateQueue.Enqueue(new UpdateData { Type = UpdateType.BelongingObtained, Payload = payload });
            TryStartProcessing(payload.SequenceId);
        }

        private void TryStartProcessing(int sequenceId)
        {
            if (!isProcessing)
            {
                isProcessing = true;
                presentationManager.Enqueue(sequenceId, PresentationPriority.CollectionUpdate, ProcessUpdateQueueRoutine());
            }
        }

        private IEnumerator ProcessUpdateQueueRoutine()
        {
            yield return OpenPanelPresentation();

            while (updateQueue.Count > 0)
            {
                var data = updateQueue.Dequeue();
                yield return ShowItemPresentation(data);
            }

            yield return ClosePanelPresentation();

            isProcessing = false; 
        }

        private IEnumerator OpenPanelPresentation()
        {
            panelView.SetActive(true);
            yield return null; 
        }

        private IEnumerator ClosePanelPresentation()
        {
            panelView.SetActive(false);
            yield return null;
        }

        private IEnumerator ShowItemPresentation(UpdateData data)
        {
            isUserConfirmed = false;

            switch (data.Type)
            {
                case UpdateType.CardObtained:
                    cardUpdateView.OnObatined((CardObtained)data.Payload, OnUpdateConfirmed);
                    break;
                case UpdateType.CardRemoved:
                    cardUpdateView.OnRemoved((CardRemoved)data.Payload, OnUpdateConfirmed);
                    break;
                case UpdateType.BelongingObtained:
                    belongingsUpdateView.OnObtained((BelongingsObtained)data.Payload, OnUpdateConfirmed);
                    break;
            }

            yield return new WaitUntil(() => isUserConfirmed);
        }

        public void OnUpdateConfirmed()
        {
            cardUpdateView.SetActive(false);
            belongingsUpdateView.SetActive(false);
            isUserConfirmed = true;
        }
    }
}