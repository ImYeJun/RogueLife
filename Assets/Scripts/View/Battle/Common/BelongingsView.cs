using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System.Collections.Generic;
using System;

namespace View.BattleView
{
    public class BelongingsView : ViewBehaviour<IBattleViewEvent>
    {
        [SerializeField] List<BelongingsIcon> icons;

        public override void OnInitialized()
        {
            eventBus.Subscribe<BelongingsSettled>(OnBelongingsSettled);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<BelongingsSettled>(OnBelongingsSettled);
        }

        public void OnBelongingsSettled(BelongingsSettled payload)
        {
            var belongings = payload.BeloningsBag.Belongings;

            if (icons.Count < belongings.Count)
            {
                throw new InvalidOperationException($"[BelongingsView] cannot draw more than {icons.Count} belonings but {belongings.Count} belonings were settled.");
            }

            for (int i = 0; i < icons.Count; i++)
            {
                if (i >= belongings.Count)
                {
                    icons[i].gameObject.SetActive(false);
                    continue;
                }

                icons[i].Initialize(belongings[i]);
                icons[i].Initialize(random, eventBus, presentationManager);
            }
        }
    }
}
