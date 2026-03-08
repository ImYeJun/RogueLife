using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu
{
    public class StartDeckSelectButton : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI startDeckDescriptionText;
        [SerializeField] private TextMeshProUGUI startDeckTypicalAttributeText;
        private Action onPressed;

        [Header("Presentation")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform items;
        [SerializeField] private float popUpDuration;
        [SerializeField] private float popUpDistance;
        [SerializeField] private Ease popUpEasingType;

        private Tween currentTween;

        public void OnPressed()
        {
            onPressed?.Invoke();
            onPressed = null;
        }

        public void Initialize(int sequenceId, PresentationManager presentationManager, int index, StartDeck startDeck, Action onPressed)
        {
            items.gameObject.SetActive(false);

            this.onPressed = onPressed;

            background.color = startDeck.UniqueColor;
            startDeckDescriptionText.text = startDeck.Description;
            startDeckTypicalAttributeText.text = startDeck.TypicalAttribute switch
            {
                CardAttribute.PHYSICAL => "(물리)",
                CardAttribute.MAGIC => "(마법)",
                CardAttribute.LUCK => "(행운)",
                _ => "( )"
            };

            presentationManager.Enqueue(sequenceId, PresentationPrioirty.StartDeckLoaded_BaseDeckPopUp + index, PopUpPresentation());
        }

        public IEnumerator PopUpPresentation()
        {
            currentTween?.Kill();
            items.gameObject.SetActive(true);
            
            items.offsetMin = new Vector2(0, -popUpDistance);
            items.offsetMax = new Vector2(0, -popUpDistance);

            var sequence = DOTween.Sequence();
            
            sequence.Join(DOTween.To(() => items.offsetMin, x => items.offsetMin = x, Vector2.zero, popUpDuration).SetEase(popUpEasingType));
            sequence.Join(DOTween.To(() => items.offsetMax, x => items.offsetMax = x, Vector2.zero, popUpDuration).SetEase(popUpEasingType));

            currentTween = sequence;

            yield return currentTween.WaitForCompletion();
        }
    }
}
