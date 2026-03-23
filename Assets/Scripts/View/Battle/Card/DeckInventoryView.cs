using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class DeckInventoryView : ViewBehaviour<IBattleViewEvent>
    {
        [SerializeField] private GameObject uiRoot;

        public override void OnInitialized()
        {
            SetActive(false);
        }

        public override void OnDestroy()
        {
        }

        public void SetActive(bool value)
        {
            uiRoot.gameObject.SetActive(value);
        }
    }
}
