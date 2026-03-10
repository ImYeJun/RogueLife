using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BackgroundClickZone : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<GameObject> detectorObjects;

        private List<IBackgroundClickDetector> detectors = new List<IBackgroundClickDetector>();

        private void Awake()
        {
            foreach (var obj in detectorObjects)
            {
                var detector = obj.GetComponent<IBackgroundClickDetector>();
                if (detector != null)
                {
                    detectors.Add(detector);
                }
                else
                {
                    Debug.LogWarning($"{obj.name}에 IBackgroundClickDetector가 없습니다!");
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            foreach (var detector in detectors)
            {
                detector.OnBackgroundClicked();
            }
        }
    }
} 