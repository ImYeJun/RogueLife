using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
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

        private struct CommonDiaryPresentationData
        {
            public string date;
            public string content;
            public Sprite stampImage;

            public CommonDiaryPresentationData(string date, string content, Sprite stampImage)
            {
                this.date = date;
                this.content = content;
                this.stampImage = stampImage;
            }
        }

        private struct SpecialDiaryPresentationData
        {
            public Sprite image;

            public SpecialDiaryPresentationData(Sprite image)
            {
                this.image = image;
            }
        }

        [Header("Common")]
        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private TextMeshProUGUI cotentText;
        [SerializeField] private Image stamp;
        [SerializeField] private List<StampImage> stampImages;
        [SerializeField] private GameObject returnToMainMenuButton;
        [SerializeField] private AudioData stampingSFX;
        private DiaryTextCurver textCurver;
        
        [Header("Special Diary")]
        [SerializeField] private Image specialDiaryImage;

        [Header("Presentation")]
        [SerializeField] private float characterTypeInterval;
        [SerializeField] private float contentTypeIntervalDeltaRange;
        [SerializeField] private float dateTextCompleteInterval;
        [SerializeField] private float stampPreInterval;
        [SerializeField] private float stampStartScale;
        [SerializeField] private float stampingDuration;
        [SerializeField] private Ease stampEase;
        [SerializeField] private float specialDiaryPreInterval;
        [SerializeField] private float specialDiaryImagePostInterval;
        [SerializeField] private float specialImageStartScale;
        [SerializeField] private float specialImageDrawDuration;
        [SerializeField] private Ease specialImageEase;

        [Header("Test Settings")]
        [SerializeField] private string testDate = "2026년 12월 25일";
        [SerializeField, TextArea(3, 10)] private string testContent = "<size=130%>1번째 일정 : 테스트 던전</size>\n슬라임 : 3회 조우, 3회 해결\n\n<size=130%>총 결산</size>\n남은 정신력 : 80";
        [SerializeField] private int testMentality = 80;
        [SerializeField] private SpecialDiaryData testSpecialDiaryData;

        private void Awake() {
            textCurver = GetComponent<DiaryTextCurver>();
        }

        public override void OnInitialized()
        {
            dateText.text = "";
            cotentText.text = "";
            stamp.sprite = null;
            stamp.gameObject.SetActive(false);
            specialDiaryImage.gameObject.SetActive(false);
            specialDiaryImage.sprite = null;
            returnToMainMenuButton.SetActive(false);
            
            eventBus.Subscribe<DiaryWritten>(OnDiaryWritten);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<DiaryWritten>(OnDiaryWritten);
        }

        private void OnDiaryWritten(DiaryWritten payload)
        {
            var diary = payload.Diary;
            specialDiaryImage.gameObject.SetActive(false);

            int remainMentality = diary.ScheduleHistories.Last().Value.RemainMentalityOnExit;
            var stampImage = stampImages.FirstOrDefault(stamp => remainMentality >= stamp.minMentality).image;
            
            var stringBuilder = new StringBuilder();
            int totalEnemyEncounterCount = 0;
            int totalEnemyResovledCount = 0;
            int totalEncounterIncidentCount = 0;
            stringBuilder.Append("\n");
            foreach (var historyIndex in diary.ScheduleHistories.Keys)
            {
                var history = diary.ScheduleHistories[historyIndex];
                stringBuilder.Append($"<size=130%>{historyIndex}번째 일정 : {history.Data.ScheduleName}</size>\n");

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

            stringBuilder.Append("<size=130%>총 결산</size>\n");
            stringBuilder.Append($"총 만난 적 : {totalEnemyEncounterCount}회 조우, {totalEnemyResovledCount}회 해결\n");
            stringBuilder.Append($"총 경험한 사건 : {totalEncounterIncidentCount}\n");
            stringBuilder.Append($"남은 정신력 : {remainMentality}\n");
            
            var commonPartData = new CommonDiaryPresentationData(
                date : diary.Date.ToString("yyyy년 M월 d일"),
                content : stringBuilder.ToString(),
                stampImage : stampImage
            );
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.DiaryWritten_CommonPartPresentation, PlayCommonPartWritePresentation(commonPartData));

            SpecialDiaryPresentationData specialDiaryPresentationData;
            if (diary.IsSpecial)
            {
                var specialDiaryData = diary.SpecialDiaryData;
                specialDiaryPresentationData = new SpecialDiaryPresentationData(
                    image : specialDiaryData.Image
                );
            }
            else
            {
                specialDiaryPresentationData = new SpecialDiaryPresentationData(
                    image : null
                );
            }
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.DiaryWritten_SpecialPartPresentation, PlaySpecialPartWritePresentation(specialDiaryPresentationData),
                () =>
                {
                    returnToMainMenuButton.SetActive(true);
                }
            );
        }  

        private IEnumerator TypewriteText(TextMeshProUGUI tmpText, string fullText)
        {
            if (tmpText == null) yield break;

            tmpText.text = fullText;
            tmpText.maxVisibleCharacters = 0;
            
            tmpText.ForceMeshUpdate();
            int maxCharacterCount = tmpText.textInfo.characterCount;

            int currentVisibleCount = 1;

            while (currentVisibleCount <= maxCharacterCount)
            {
                tmpText.maxVisibleCharacters = currentVisibleCount;
                
                textCurver.ApplyCurve(tmpText);

                var randomIntervalDelta = UnityEngine.Random.Range(-contentTypeIntervalDeltaRange, contentTypeIntervalDeltaRange);
                yield return new WaitForSeconds(characterTypeInterval + randomIntervalDelta);
                
                currentVisibleCount++;
            }
        }
        private IEnumerator PlayCommonPartWritePresentation(CommonDiaryPresentationData data)
        {
            stamp.gameObject.SetActive(false);
            dateText.text = "";
            cotentText.text = "";

            yield return StartCoroutine(TypewriteText(dateText, data.date));
            yield return new WaitForSeconds(dateTextCompleteInterval);
            yield return StartCoroutine(TypewriteText(cotentText, data.content));
            yield return StartCoroutine(StampingPresentation(data.stampImage));
        }

        private IEnumerator StampingPresentation(Sprite stampImage)
        {
            stamp.sprite = stampImage;

            yield return new WaitForSeconds(stampPreInterval);
            stamp.gameObject.SetActive(true);

            var sequence = DOTween.Sequence();
            sequence.Append(stamp.transform.DOScale(1, stampingDuration).From(stampStartScale).SetEase(stampEase));
            sequence.AppendCallback(() => SoundManager.Instance?.PlayeSoundEffect(stampingSFX));
            yield return sequence.WaitForCompletion();
        }

        private IEnumerator PlaySpecialPartWritePresentation(SpecialDiaryPresentationData data)
        {
            specialDiaryImage.sprite = data.image;
            yield return new WaitForSeconds(specialDiaryPreInterval);
            specialDiaryImage.gameObject.SetActive(true);

            var sequence = DOTween.Sequence();
            sequence.Append(specialDiaryImage.transform.DOScale(1, specialImageDrawDuration).From(specialImageStartScale).SetEase(specialImageEase));
            sequence.AppendCallback(() => SoundManager.Instance?.PlayeSoundEffect(stampingSFX));
            
            yield return sequence.WaitForCompletion();
            yield return new WaitForSeconds(specialDiaryImagePostInterval);
        }


        [ContextMenu("Test Common Presentation")]
        public void TestCommonPresentation()
        {
            StopAllCoroutines();
            StartCoroutine(TestCommonRoutine());
        }

        [ContextMenu("Test Special Presentation")]
        public void TestSpecialPresentation()
        {
            StopAllCoroutines();
            StartCoroutine(TestSpecialRoutine());
        }

        [ContextMenu("Test Full (Common -> Special/Normal) Presentation")]
        public void TestFullPresentation()
        {
            StopAllCoroutines();
            StartCoroutine(TestFullRoutine());
        }

        private IEnumerator TestCommonRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            specialDiaryImage.gameObject.SetActive(false);

            Sprite testStamp = null;
            if (stampImages != null && stampImages.Count > 0)
            {
                testStamp = stampImages.FirstOrDefault(stamp => testMentality >= stamp.minMentality).image;
            }

            var commonData = new CommonDiaryPresentationData(testDate, testContent, testStamp);
            yield return StartCoroutine(PlayCommonPartWritePresentation(commonData));
        }

        private IEnumerator TestSpecialRoutine()
        {
            yield return new WaitForSeconds(0.5f);

            SpecialDiaryPresentationData specialData;
            
            if (testSpecialDiaryData != null)
            {
                specialData = new SpecialDiaryPresentationData(
                    testSpecialDiaryData.Image
                );
            }
            else
            {
                specialData = new SpecialDiaryPresentationData(
                    null
                );
                Debug.Log("[DiaryWriterView] Test Special Diary Data가 없으므로 Common Diary용 연출로 진행합니다.");
            }

            yield return StartCoroutine(PlaySpecialPartWritePresentation(specialData));
        }

        private IEnumerator TestFullRoutine()
        {
            bool isSpecial = testSpecialDiaryData != null;
            specialDiaryImage.gameObject.SetActive(false);
            returnToMainMenuButton.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            Sprite testStamp = null;
            if (stampImages != null && stampImages.Count > 0)
            {
                testStamp = stampImages.FirstOrDefault(stamp => testMentality >= stamp.minMentality).image;
            }
            var commonData = new CommonDiaryPresentationData(testDate, testContent, testStamp);

            SpecialDiaryPresentationData specialData;
            
            if (isSpecial)
            {
                specialData = new SpecialDiaryPresentationData(
                    testSpecialDiaryData.Image
                );
            }
            else
            {
                specialData = new SpecialDiaryPresentationData(
                    null
                );
                Debug.Log("[DiaryWriterView] Test Special Diary Data가 없으므로 Full Test의 후반부는 Common Diary용 연출로 진행합니다.");
            }

            yield return StartCoroutine(PlayCommonPartWritePresentation(commonData));
            yield return StartCoroutine(PlaySpecialPartWritePresentation(specialData));
            returnToMainMenuButton.SetActive(true);
        }
    }
}