using TMPro;
using UnityEngine;

namespace View.BattleView
{
    public class InspectorSubPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private RectTransform itemContainer;

        public string Header { get => header.text; set => header.text = value; }
        public RectTransform ItemContainer => itemContainer;
    }
}