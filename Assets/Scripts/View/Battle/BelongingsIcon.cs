using System;
using UnityEngine;
using UnityEngine.UI;

namespace View.BattleView
{
    public class BelongingsIcon : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Initialize(BattleBelongings belongings)
        {
            image.sprite = belongings.Image;
        }
    }
}