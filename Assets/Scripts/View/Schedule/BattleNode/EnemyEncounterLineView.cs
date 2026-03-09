using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.BattleNodes
{
    public class EnemyEncounterLineView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI line;
        private string selectedLine;

        public void Initiate(System.Random random, IReadOnlyList<string> encounterLines)
        {
            if (encounterLines.Count == 0)
            {
                selectedLine = "";
            }
            else
            {
                selectedLine = encounterLines[random.Next(encounterLines.Count)];
            }

            line.text = selectedLine;
        }
    }
}