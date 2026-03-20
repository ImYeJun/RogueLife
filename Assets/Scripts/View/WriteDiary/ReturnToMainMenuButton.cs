using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.WriteDiaryView;

namespace View.WriteDiaryView
{
    public class ReturnToMainMenuButton : InteractableViewBehaviour<IWriteDiaryViewEvent, IWriteDiaryViewCommander>
    {
        public override void OnInitialized()
        {
        }

        public override void OnDestroy()
        {
        }

        public void OnPressed()
        {
            commander.RequestReturnToMainMenu();
        }
    }
}
