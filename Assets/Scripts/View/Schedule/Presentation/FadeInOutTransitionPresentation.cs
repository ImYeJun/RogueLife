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
        private static readonly int SeedID = Shader.PropertyToID("_Seed");
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");

        [SerializeField] private RectTransform playerView;
        [SerializeField] private PlayerImageView playerImageView;
        [SerializeField] private GameObject foreground;
        [SerializeField] private Material tilingMaterial;


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

        [Header("Battle Engage")]
        [SerializeField] private float engageFadeDuration;
        [SerializeField] private Ease engageFadeEasingType;

        [Header("Returned From Battle")]
        [SerializeField] private float returnFadeDuration;
        [SerializeField] private Ease returnFadeEasingType;

        private Image foregroundImage;
        private RectTransform foregroundTransform;

        public override void OnInitialized()
        {
            foregroundImage = foreground.GetComponent<Image>();
            foregroundTransform = foreground.GetComponent<RectTransform>();
            
            foregroundTransform.gameObject.SetActive(true);

            eventBus.Subscribe<NodeEntered>(OnNodeEntered);
            eventBus.Subscribe<NodeExited>(OnNodeExited);
            eventBus.Subscribe<ReturnedFromBattle>(OnReturnedFromBattle);
            eventBus.Subscribe<BattleEngaged>(OnBattleEngaged);
        }
        
        public override void OnDestroy()
        {
            KillActiveTweens(); 

            eventBus?.Unsubscribe<NodeEntered>(OnNodeEntered);
            eventBus?.Unsubscribe<NodeExited>(OnNodeExited);
            eventBus?.Unsubscribe<ReturnedFromBattle>(OnReturnedFromBattle);
            eventBus?.Unsubscribe<BattleEngaged>(OnBattleEngaged);
        }

        private void KillActiveTweens()
        {
            playerView?.DOKill();
            foregroundTransform?.DOKill();
            foregroundImage?.DOKill();
            if (tilingMaterial != null) 
            {
                tilingMaterial.DOKill();
            }
        }

        public void OnBattleEngaged(BattleEngaged payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleEngaged_FadeIn, OnBattleEngagedPresentation());
        }
        private IEnumerator OnBattleEngagedPresentation()
        {
            KillActiveTweens();

            foregroundTransform.sizeDelta = new Vector2(1920f, 1080f);
            foregroundImage.material = tilingMaterial;
            foreground.SetActive(true);
            
            int seed = random.Next(1000 + 1);
            tilingMaterial.SetFloat(SeedID, seed);
            tilingMaterial.SetFloat(ProgressID, 0);
            yield return tilingMaterial.DOFloat(1, ProgressID, engageFadeDuration).SetEase(engageFadeEasingType).WaitForCompletion();

            foregroundImage.material = null;
        }

        private void OnReturnedFromBattle(ReturnedFromBattle payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ReturnedFromBattle_FadeOut, ReturnedFromBattlePresentation());
        }

        private IEnumerator ReturnedFromBattlePresentation()
        {
            KillActiveTweens();

            playerView.anchoredPosition = new Vector2(enterMoveDistance, 0); 
            playerImageView.SetIdleView();
            foreground.SetActive(true);
            
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

            foreground.SetActive(false);
            foregroundImage.color = new Color(0, 0, 0, 1f);
        }

        public void OnNodeEntered(NodeEntered payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_MovePlayer, EnterNodePresentation());
        }
        
        public IEnumerator EnterNodePresentation()
        {
            KillActiveTweens();

            foreground.SetActive(true);
            playerImageView.SetWalkView();
            
            playerView.anchoredPosition = Vector2.zero;
            var targetPosition = new Vector2(enterMoveDistance, 0);

            foregroundTransform.pivot = new Vector2(1f, 0.5f);
            foregroundTransform.anchoredPosition = new Vector2(1920f, 0f); 
            foregroundTransform.sizeDelta = new Vector2(1920f, 1080f);

            var tween = DOTween.Sequence();
            
            var playerTween = playerView.DOAnchorPos(targetPosition, enterPlayerMoveDuration)
                .SetEase(enterPlayerEasingType)
                .OnComplete(() => 
                { 
                    playerView.anchoredPosition = targetPosition; 
                    playerImageView.SetIdleView(); 
                });
                
            var foregroundTween = foregroundTransform.DOSizeDelta(new Vector2(0f, 1080f), enterForegroundMoveDuration)
                .SetEase(enterForegroundEasingType);
            
            tween.Join(playerTween).Join(foregroundTween);

            yield return tween.WaitForCompletion();

            foreground.SetActive(false);
        }

        public void OnNodeExited(NodeExited payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeExit_MovePlayer, ExitNodePresentation());
        }
        
        public IEnumerator ExitNodePresentation()
        {
            KillActiveTweens();

            foreground.SetActive(true);
            playerImageView.SetWalkView();

            playerView.anchoredPosition = new Vector2(enterMoveDistance, 0);
            Vector2 targetPosition = new Vector2(playerView.anchoredPosition.x + exitMoveDistance, playerView.anchoredPosition.y);

            foregroundTransform.pivot = new Vector2(0f, 0.5f);
            foregroundTransform.anchoredPosition = Vector2.zero; 
            foregroundTransform.sizeDelta = new Vector2(0f, 1080f);

            var tween = DOTween.Sequence();
            
            var playerTween = playerView.DOAnchorPos(targetPosition, exitPlayerMoveDuration)
                .SetEase(exitPlayerEasingType)
                .OnComplete(() => 
                { 
                    playerView.anchoredPosition = targetPosition; 
                    playerImageView.SetIdleView(); 
                });
                
            var foregroundTween = foregroundTransform.DOSizeDelta(new Vector2(1920f, 1080f), exitForegroundMoveDuration)
                .SetEase(exitForegroundEasingType);
            
            tween.Join(playerTween).Join(foregroundTween);

            yield return tween.WaitForCompletion();
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

        [ContextMenu("Play Engage Battle Presentation")]
        public void TestOnEnageBattle()
        {
            presentationManager.Enqueue(0, PresentationPriority.BattleEngaged_FadeIn, OnBattleEngagedPresentation());
            foreground.SetActive(true);
        }
#endif
    }
}