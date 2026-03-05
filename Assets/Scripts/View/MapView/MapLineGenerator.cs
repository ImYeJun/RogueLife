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

            // 💡 1. 서로 다른 부모를 가진 노드들의 글로벌 좌표(position)를 
            // 선을 그릴 부모(linesParent) 기준의 로컬 좌표로 통일시켜 줍니다.
            Vector3 localFrom = linesParent.InverseTransformPoint(from.position);
            Vector3 localTo = linesParent.InverseTransformPoint(to.position);

            // 💡 2. 통일된 로컬 좌표를 바탕으로 진짜 거리와 방향을 구합니다.
            Vector3 direction = localTo - localFrom;
            float distance = direction.magnitude;

            // 3. 각도 계산 (기본적으로 오른쪽을 0도로 계산함)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            lineRect.localPosition = localFrom;

            // 💡 4. [중요] 사용하신 이미지가 '세로' 방향이므로 
            // 오른쪽(0도)을 바라보는 Atan2 기준에서 90도를 빼주어야 정확히 앞을 바라봅니다.
            lineRect.localRotation = Quaternion.Euler(0, 0, angle - 90f); 

            // 💡 5. [중요] 이미지가 '세로' 방향이므로, 거리를 Height(Y)에 넣고 두께를 Width(X)에 넣습니다.
            lineRect.sizeDelta = new Vector2(lineWidth, distance);
            
            lineRect.SetAsFirstSibling();

            lineObjects.Add(lineObj);
        }

        public void ClearLines()
        {
            foreach (var line in lineObjects)
            {
                Destroy(line);
            }
            lineObjects.Clear(); // 리스트도 비워줘야 메모리가 쌓이지 않습니다.
        }
    }
}