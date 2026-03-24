using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleRewardButton : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private IBattleReward reward;
    private Action onButtonSelected;
    private IScheduleViewCommander commander;

    [Header("Presentation")]
    [SerializeField] private float fadeDuration;
    [SerializeField] private Ease fadeEase;
    [SerializeField] private float showDuration;
    [SerializeField] private float showDistance;
    [SerializeField] private Ease showEase;

    public void Initiate(IBattleReward reward, Action onButtonSelected, IScheduleViewCommander commander)
    {
        this.reward = reward;
        this.onButtonSelected = onButtonSelected;
        this.commander = commander;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup.alpha = 0;

        if (reward is null) { return; }
        text.text = $"{reward.Name} 획득하기";
    }

    public Tween ShowPresentation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(canvasGroup.DOFade(1, fadeDuration).From(0).SetEase(fadeEase).SetLink(gameObject));
        sequence.Join(rectTransform.DOAnchorPosX(showDistance, showDuration).From(true).SetEase(showEase).SetLink(gameObject));

        return sequence;
    }

    public void OnPressed()
    {
        reward?.Resolve(commander);

        onButtonSelected.Invoke();
    }
}