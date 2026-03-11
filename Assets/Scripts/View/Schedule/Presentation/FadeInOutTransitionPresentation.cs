using System;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

namespace View.ScheduleView.Presentation
{
    public class FadeInOutTransitionPresentation : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private RectTransform playerView;
        [SerializeField] private PlayerImageView playerImageView;
        [SerializeField] private RectTransform foreground;

        [Header("On Node Enter")]
        [SerializeField] private float enterMoveDistance;
        [SerializeField] private float enterPlayerMoveDuration;
        [SerializeField] private Ease enterPlayerEasingType;
        [SerializeField] private float enterForegroundMoveDuration;
        [SerializeField] private Ease enterForegroundEasingType;

        [Header("On Node Exit")]
        [SerializeField] private float exitMoveDistance;
        [SerializeField] private float exitPlayerMoveDuration;
        [SerializeField] private Ease exitPlayerEasingType;
        [SerializeField] private float exitForegroundMoveDuration;
        [SerializeField] private Ease exitForegroundEasingType;

        [Header("Returned From Battle")]
        [SerializeField] private float returnFadeDuration;
        [SerializeField] private Ease returnFadeEasingType;

        public override void OnInitialized()
        {
            foreground.gameObject.SetActive(true);

            eventBus.Subscribe<NodeEntered>(OnNodeEntered);
            eventBus.Subscribe<NodeExited>(OnNodeExited);
            eventBus.Subscribe<ReturnedFromBattle>(OnReturnedFromBattle);
        }
        
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<NodeEntered>(OnNodeEntered);
            eventBus?.Unsubscribe<NodeExited>(OnNodeExited);
            eventBus?.Unsubscribe<ReturnedFromBattle>(OnReturnedFromBattle);
        }

        private void OnReturnedFromBattle(ReturnedFromBattle payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ReturnedFromBattle_FadeOut, ReturnedFromBattlePresentation());
        }

        private IEnumerator ReturnedFromBattlePresentation()
        {
            playerView.anchoredPosition = new Vector2(enterMoveDistance, 0); 
            playerImageView.SetIdleView();

            foreground.gameObject.SetActive(true);
            
            Image foregroundImage = foreground.GetComponent<Image>();
            if (foregroundImage != null)
            {
                foregroundImage.color = new Color(0, 0, 0, 1f);

                var tween = foregroundImage.DOFade(0f, returnFadeDuration).SetEase(returnFadeEasingType);
                
                yield return tween.WaitForCompletion();
            }
            else
            {
                Debug.LogWarning("[FadeInOutTransitionPresentation] Foreground object does not have an Image component for fading.");
                yield return null;
            }

            foreground.gameObject.SetActive(false);
            foregroundImage.color = new Color(0, 0, 0, 1f);
        }

        public void OnNodeEntered(NodeEntered payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_MovePlayer, EnterNodePresentation());
        }
        
        public IEnumerator EnterNodePresentation()
        {
            foreground.gameObject.SetActive(true);
            playerImageView.SetWalkView();
            
            playerView.anchoredPosition = Vector2.zero;
            var targetPosition = new Vector2(enterMoveDistance, 0);

            foreground.pivot = new Vector2(1f, 0.5f);
            foreground.anchoredPosition = new Vector2(1920f, 0f); 
            foreground.sizeDelta = new Vector2(1920f, 1080f);

            var tween = DOTween.Sequence();
            
            var playerTween = playerView.DOAnchorPos(targetPosition, enterPlayerMoveDuration)
                .SetEase(enterPlayerEasingType)
                .OnComplete(() => 
                { 
                    playerView.anchoredPosition = targetPosition; 
                    playerImageView.SetIdleView(); 
                });
                
            var foregroundTween = foreground.DOSizeDelta(new Vector2(0f, 1080f), enterForegroundMoveDuration)
                .SetEase(enterForegroundEasingType);
            
            tween.Join(playerTween).Join(foregroundTween);

            yield return tween.WaitForCompletion();

            foreground.gameObject.SetActive(false);
        }

        public void OnNodeExited(NodeExited payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeExit_MovePlayer, ExitNodePresentation());
        }
        
        public IEnumerator ExitNodePresentation()
        {
            foreground.gameObject.SetActive(true);
            playerImageView.SetWalkView();

            playerView.anchoredPosition = new Vector2(enterMoveDistance, 0);
            Vector2 targetPosition = new Vector2(playerView.anchoredPosition.x + exitMoveDistance, playerView.anchoredPosition.y);

            foreground.pivot = new Vector2(0f, 0.5f);
            foreground.anchoredPosition = Vector2.zero; 
            foreground.sizeDelta = new Vector2(0f, 1080f);

            var tween = DOTween.Sequence();
            
            var playerTween = playerView.DOAnchorPos(targetPosition, exitPlayerMoveDuration)
                .SetEase(exitPlayerEasingType)
                .OnComplete(() => 
                { 
                    playerView.anchoredPosition = targetPosition; 
                    playerImageView.SetIdleView(); 
                });
                
            var foregroundTween = foreground.DOSizeDelta(new Vector2(1920f, 1080f), exitForegroundMoveDuration)
                .SetEase(exitForegroundEasingType);
            
            tween.Join(playerTween).Join(foregroundTween);

            yield return tween.WaitForCompletion();

            foreground.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        [ContextMenu("Play Node Enter Presentation")]
        public void TestOnNodeEntered()
        {
            presentationManager.Enqueue(0, PresentationPriority.NodeEnter_MovePlayer, EnterNodePresentation());
        }

        [ContextMenu("Play Node Exit Presentation")]
        public void TestOnNodeExited()
        {
            presentationManager.Enqueue(0, PresentationPriority.NodeExit_MovePlayer, ExitNodePresentation());
        }

        [ContextMenu("Play Returned From Battle Presentation")]
        public void TestOnReturnedFromBattle()
        {
            presentationManager.Enqueue(0, PresentationPriority.ReturnedFromBattle_FadeOut, ReturnedFromBattlePresentation());
        }
#endif
    }
}