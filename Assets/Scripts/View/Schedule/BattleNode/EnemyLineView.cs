using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.BattleNodes
{
    public class EnemyLineView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI line;

        public void SetLine(System.Random random, IReadOnlyList<string> lines)
        {
            string selectedLine;
            if (lines.Count == 0)
            {
                selectedLine = "";
            }
            else
            {
                selectedLine = lines[random.Next(lines.Count)];
            }

            line.text = selectedLine;
        }
    }
}