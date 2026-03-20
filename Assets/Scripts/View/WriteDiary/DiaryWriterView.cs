using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.Core;
using ViewEvent.WriteDiaryView;

namespace View.WriteDiaryView
{
    public class DiaryWriterView : ViewBehaviour<IWriteDiaryViewEvent>
    {
        [Serializable]
        public struct StampImage
        {
            public int minMentality;
            public Sprite image;
        }

        [Header("Common")]
        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private TextMeshProUGUI cotentText;
        [SerializeField] private Image stamp;
        [SerializeField] private List<StampImage> stampImages;
        
        [Header("Special Diary")]
        [SerializeField] private GameObject normalIndicator;
        [SerializeField] private Image specialDiaryImage;
        [SerializeField] private TextMeshProUGUI specialDiaryName;
        [SerializeField] private TextMeshProUGUI specialDiaryDescription;
        public override void OnInitialized()
        {
            eventBus.Subscribe<DiaryWritten>(OnDiaryWritten);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<DiaryWritten>(OnDiaryWritten);
        }

        private void OnDiaryWritten(DiaryWritten payload)
        {
            var diary = payload.Diary;
            dateText.text = diary.Date.ToString("yyyy년 M월 d일");

            int remainMentality = diary.ScheduleHistories.Last().Value.RemainMentalityOnExit;
            var stampImage = stampImages.FirstOrDefault(stamp => remainMentality >= stamp.minMentality);
            if (stampImage.image != null)
            {
                stamp.sprite = stampImage.image;
            }

            var stringBuilder = new StringBuilder();
            int totalEnemyEncounterCount = 0;
            int totalEnemyResovledCount = 0;
            int totalEncounterIncidentCount = 0;
            foreach (var historyIndex in diary.ScheduleHistories.Keys)
            {
                var history = diary.ScheduleHistories[historyIndex];
                stringBuilder.Append($"{historyIndex}번째 일정 : {history.Data.ScheduleName}\n");

                foreach (var encounteredEnemy in history.EncounterEnemies)
                {
                    var data = encounteredEnemy.Key;
                    var encounterInfo = encounteredEnemy.Value;
                    stringBuilder.Append($"{data.EnemyName} : {encounterInfo.encounerCount}회 조우, {encounterInfo.resolvedCount}회 해결\n");

                    totalEnemyEncounterCount += encounterInfo.encounerCount;
                    totalEnemyResovledCount += encounterInfo.resolvedCount;
                }

                foreach (var encounteredIncident in history.EncounterIncidents)
                {
                    var data = encounteredIncident.Key;
                    var encounterCount = encounteredIncident.Value;
                    stringBuilder.Append($"{data.IncidentName} : {encounterCount}회 경험\n");

                    totalEncounterIncidentCount += encounterCount;
                }
            }

            stringBuilder.Append("총 결산\n");
            stringBuilder.Append($"총 만난 적 : {totalEnemyEncounterCount}회 조우, {totalEnemyResovledCount}회 해결\n");
            stringBuilder.Append($"총 경험한 사건 : {totalEncounterIncidentCount}\n");
            stringBuilder.Append($"남은 정신력 : {remainMentality}\n");

            cotentText.text = stringBuilder.ToString();

            if (diary.IsSpecial)
            {
                normalIndicator.SetActive(false);
                var specialDiaryData = diary.SpecialDiaryData;
                specialDiaryImage.sprite = specialDiaryData.Image;
                specialDiaryName.text = specialDiaryData.Name;
                specialDiaryDescription.text = specialDiaryData.Description;
            }
            else
            {
                normalIndicator.SetActive(true);
                specialDiaryImage.sprite = null;
                specialDiaryName.text = "";
                specialDiaryDescription.text = "";
            }
        }
    }
}
