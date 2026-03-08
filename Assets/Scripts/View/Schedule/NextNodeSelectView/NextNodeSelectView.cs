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
        private const string LEFT_NODE_MAIN_DESCRIPTION = "왼쪽으로 가기";
        private const string MIDDLE_NODE_MAIN_DESCRIPTION = "직진하기";
        private const string RIGHT_NODE_MAIN_DESCRIPTION = "오른쪽으로 가기";

        [Header("Behaviour")]
        [SerializeField] private GameObject uiRoot;

        [SerializeField] private Transform nextNodeButtonsContainer;
        [SerializeField] private GameObject nextNodeButtonPrefab;
        private IObjectPool<NextNodeButton> buttonPool;
        private List<NextNodeButton> activeButtons = new List<NextNodeButton>();

        [Header("Presentation")]
        [SerializeField] private float durarion;
        [SerializeField] private CanvasGroup panelCanvasGroup;

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);

            buttonPool = new ObjectPool<NextNodeButton>(
                createFunc : () =>
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
                actionOnGet : (button) => { button.gameObject.SetActive(true); },
                actionOnRelease : (button) => { button.gameObject.SetActive(false); },
                actionOnDestroy : (button) => { Destroy(button.gameObject); },
                defaultCapacity : 3,
                maxSize : 3
            );
            
            activeButtons.Clear();

            eventBus.Subscribe<NextNodeSelectRequested>(OnNextNodeSelectRequested);
            eventBus.Subscribe<NodeExited>(OnNodeExited);
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

            switch (nextNodes.Count)
            {
                case 1:
                    OnSingleNode(nextNodes[0]);
                    break;
                case 2:
                    OnDoubleNodes(nextNodes[0], nextNodes[1]);
                    break;
                case 3:
                    OnTripleNodes(nextNodes[0], nextNodes[1], nextNodes[2]);
                    break;
                default:
                    throw new InvalidOperationException("[NextNodeSelectView] Expecting next nodes count is either 1, 2, 3");
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPrioirty.NodeSelect_OpenPanel, OpenPanelPresentation());
        }
        private void OnSingleNode(Node node)
        {
            var button = buttonPool.Get();
            button.Initiate(MIDDLE_NODE_MAIN_DESCRIPTION, node, OnNextNodeSelected);
            activeButtons.Add(button);
        }
        
        private void OnDoubleNodes(Node node1, Node node2)
        {
            var firstButton = buttonPool.Get();
            firstButton.Initiate(LEFT_NODE_MAIN_DESCRIPTION, node1, OnNextNodeSelected);
            activeButtons.Add(firstButton);
            
            var secondButton = buttonPool.Get();
            secondButton.Initiate(RIGHT_NODE_MAIN_DESCRIPTION, node2, OnNextNodeSelected);
            activeButtons.Add(secondButton);
        }
        
        private void OnTripleNodes(Node node1, Node node2, Node node3)
        {
            var firstButton = buttonPool.Get();
            firstButton.Initiate(LEFT_NODE_MAIN_DESCRIPTION, node1, OnNextNodeSelected);
            activeButtons.Add(firstButton);

            var secondButton = buttonPool.Get();
            secondButton.Initiate(MIDDLE_NODE_MAIN_DESCRIPTION, node2, OnNextNodeSelected);
            activeButtons.Add(secondButton);

            var thirdButton = buttonPool.Get();
            thirdButton.Initiate(RIGHT_NODE_MAIN_DESCRIPTION, node3, OnNextNodeSelected);
            activeButtons.Add(thirdButton);
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
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.alpha = 0;

            uiRoot.SetActive(true);

            var tween = panelCanvasGroup.DOFade(1.0f, durarion);
            yield return tween.WaitForCompletion();

            panelCanvasGroup.interactable = true;
            panelCanvasGroup.alpha = 1;
        }
    }
}