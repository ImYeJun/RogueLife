using System;
using System.Collections.Generic;
using UnityEngine;

namespace View.ScheduleView.Map
{
    public class MapLineGenerator : MonoBehaviour
    {
        [Header("Line Settings")]
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private Transform linesParent;
        [SerializeField] private float lineWidth = 5f;  
        private List<GameObject> lineObjects = new List<GameObject>();

        public void DrawLine(Transform from, Transform to)
        {
            GameObject lineObj = Instantiate(linePrefab, linesParent);
            RectTransform lineRect = lineObj.GetComponent<RectTransform>();

            Vector3 localFrom = linesParent.InverseTransformPoint(from.position);
            Vector3 localTo = linesParent.InverseTransformPoint(to.position);

            Vector3 direction = localTo - localFrom;
            float distance = direction.magnitude;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            lineRect.localPosition = localFrom;

            lineRect.localRotation = Quaternion.Euler(0, 0, angle); 

            lineRect.sizeDelta = new Vector2(distance, lineWidth);
            
            lineRect.SetAsFirstSibling();

            lineObjects.Add(lineObj);
        }

        public void ClearLines()
        {
            foreach (var line in lineObjects)
            {
                Destroy(line);
            }
            lineObjects.Clear();
        }
    }
}