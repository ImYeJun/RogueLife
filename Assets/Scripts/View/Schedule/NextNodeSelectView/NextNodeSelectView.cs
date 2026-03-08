using System;
using System.Collections;
using System.Collections.Generic;
using Battle.BattleResultCommands;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.NextNodeSelectView
{
    public class NextNodeSelectView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [Header("Behaviour")]
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private Transform nextNodeButtonsContainer;
        [SerializeField] private GameObject nextNodeButtonPrefab;
        
        private IObjectPool<NextNodeButton> buttonPool;
        private List<NextNodeButton> activeButtons = new List<NextNodeButton>();

        [Header("Presentation")]
        [SerializeField] private float duration; 
        [SerializeField] private CanvasGroup panelCanvasGroup;
        [SerializeField] private Ease panelEasingType;

        private Tween panelTween;

        private readonly NextNodeButton.NodeDirection[][] DirectionMap = {
            Array.Empty<NextNodeButton.NodeDirection>(), 
            new[] { NextNodeButton.NodeDirection.Middle },
            new[] { NextNodeButton.NodeDirection.Left, NextNodeButton.NodeDirection.Right },
            new[] { NextNodeButton.NodeDirection.Left, NextNodeButton.NodeDirection.Middle, NextNodeButton.NodeDirection.Right }
        };

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);

            buttonPool = new ObjectPool<NextNodeButton>(
                createFunc: () =>
                {
                    var button = Instantiate(nextNodeButtonPrefab, nextNodeButtonsContainer);
                    button.SetActive(false);
                    
                    var comp = button.GetComponent<NextNodeButton>();
                    if (comp == null) 
                    {
                        Debug.LogError("[NextNodeSelectView] 프리팹에 NextNodeButton 컴포넌트가 없습니다!");
                    }
                    return comp;
                },
                actionOnGet: (button) => { button.gameObject.SetActive(true); },
                actionOnRelease: (button) => { button.gameObject.SetActive(false); },
                actionOnDestroy: (button) => { Destroy(button.gameObject); },
                defaultCapacity: 3,
                maxSize: 3
            );
            
            activeButtons.Clear();

            eventBus?.Subscribe<NextNodeSelectRequested>(OnNextNodeSelectRequested);
            eventBus?.Subscribe<NodeExited>(OnNodeExited);
        }
        
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<NextNodeSelectRequested>(OnNextNodeSelectRequested);
            eventBus?.Unsubscribe<NodeExited>(OnNodeExited);
        }

        public void OnNextNodeSelectRequested(NextNodeSelectRequested payload)
        {
            uiRoot.SetActive(false);
            var nextNodes = payload.NextNodes;

            if (nextNodes.Count < 1 || nextNodes.Count > 3)
            {
                throw new InvalidOperationException($"[NextNodeSelectView] Expecting next nodes count between 1 and 3, but got {nextNodes.Count}");
            }

            var directions = DirectionMap[nextNodes.Count];
            for (int i = 0; i < nextNodes.Count; i++)
            {
                var button = buttonPool.Get();
                button.transform.SetAsLastSibling();
                
                button.Initiate(payload.SequenceId, presentationManager, directions[i], nextNodes[i], OnNextNodeSelected);
                activeButtons.Add(button);
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeSelect_OpenPanel, OpenPanelPresentation());
        }

        public void OnNextNodeSelected(Node nextNode)
        {
            commander.SettleNextNode(nextNode);
        }

        public void OnNodeExited(NodeExited payload)
        {
            foreach (var button in activeButtons)
            {
                buttonPool.Release(button);
            }

            activeButtons.Clear();
            uiRoot.SetActive(false); 
        }

        public IEnumerator OpenPanelPresentation()
        {
            panelTween?.Kill();

            panelCanvasGroup.alpha = 0;
            uiRoot.SetActive(true);

            panelTween = panelCanvasGroup.DOFade(1.0f, duration).SetEase(panelEasingType);
            yield return panelTween.WaitForCompletion();
        }

#if UNITY_EDITOR
        [ContextMenu("Test: Open panel without button")]
        public void TestOpenPanelPresentation()
        {
            presentationManager.Enqueue(0, PresentationPriority.NodeSelect_OpenPanel, OpenPanelPresentation());
        }

        [ContextMenu("Test: Open panel wtth 3 buttons")]
        public void TestFullPresentation()
        {
            foreach (var button in activeButtons)
            {
                buttonPool.Release(button);
            }
            activeButtons.Clear();

            var dummyNodes = new List<Node>
            {
                new BattleNode(Guid.NewGuid(), null, new List<EnemyDataSlot>()),
                new IncidentNode(Guid.NewGuid(), null),
                new TransactionNode(Guid.NewGuid())
            };

            uiRoot.SetActive(false);

            var directions = DirectionMap[3];
            for (int i = 0; i < 3; i++)
            {
                var button = buttonPool.Get();
                button.transform.SetAsLastSibling();

                button.Initiate(0, presentationManager, directions[i], dummyNodes[i], OnNextNodeSelected);
                activeButtons.Add(button);
            }

            presentationManager.Enqueue(0, PresentationPriority.NodeSelect_OpenPanel, OpenPanelPresentation());
        }
#endif
    }
}