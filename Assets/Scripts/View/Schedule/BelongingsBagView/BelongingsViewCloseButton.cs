using UnityEngine;

namespace View.ScheduleView.BelongingsBag
{
    public class BelongingsViewCloseButton : MonoBehaviour {
        [SerializeField] private GameObject belongingsView;

        public void OnPressed()
        {
            belongingsView.SetActive(false);
        }
    }
}