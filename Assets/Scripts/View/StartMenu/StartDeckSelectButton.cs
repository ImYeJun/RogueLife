using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu
{
    public class StartDeckSelectButton : InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI startDeckDescriptionText;
        [SerializeField] private TextMeshProUGUI startDeckTypicalAttributeText;

        private StartDeck startDeck;

        public override void OnInitialized()
        {
            // TODO: 이벤트 구독 (예: eventBus.Subscribe<T>(Method);)
        }

        public override void OnDestroy()
        {
            // TODO: 이벤트 구독 해제 (예: eventBus.Unsubscribe<T>(Method);)
        }

        public void OnPressed()
        {
            commander.FixStartDeck(startDeck);
        }

        public void SetStartDeck(StartDeck startDeck)
        {
            this.startDeck = startDeck;

            background.color = startDeck.UniqueColor;
            startDeckDescriptionText.text = startDeck.Description;
            startDeckTypicalAttributeText.text = startDeck.TypicalAttribute switch
            {
                CardAttribute.PHYSICAL => "(물리)",
                CardAttribute.MAGIC => "(마법)",
                CardAttribute.LUCK => "(행운)",
                _ => "( )"
            };
        }
    }
}
