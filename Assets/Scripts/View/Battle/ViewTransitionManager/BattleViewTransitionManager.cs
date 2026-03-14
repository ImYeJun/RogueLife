using System;
using UnityEngine;

namespace View.BattleView
{
    public class BattleViewTransitionManager : MonoBehaviour
    {
        [SerializeField] private BattleEntityInspectorView entityInspectorView;

        public void InspectEntity(IInspectable inspectable)
        {
            entityInspectorView.InspectEntity(inspectable);
        }
    }
}
