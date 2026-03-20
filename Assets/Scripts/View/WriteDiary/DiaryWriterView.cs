using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.WriteDiaryView;

namespace View.WriteDiaryView
{
    public class DiaryWriterView : ViewBehaviour<IWriteDiaryViewEvent>
    {
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
            Debug.Log(payload.Diary);
        }
    }
}
