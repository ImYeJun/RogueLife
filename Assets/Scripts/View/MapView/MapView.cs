using System;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Map
{
    public class MapView : ViewBehaviour<IScheduleViewEvent>
    {
        private IReadOnlyDictionary<int, List<Node>> map;

        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject mapLayerViewPrefab;
        [SerializeField] private GameObject mapNodeIconViewPrefab;

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
        }

        private void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            map = payload.Schedule.Map;

            DrawMap();
        }
        public void OnViewOpened()
        {
            gameObject.SetActive(true);
        }

        private void DrawMap()
        {
            foreach (var layer in map.Values)
            {
                var layerView = Instantiate(mapLayerViewPrefab, scrollContent);

                foreach (var node in layer)
                {
                    var nodeIconGameObject = Instantiate(mapNodeIconViewPrefab, layerView.transform);
                    
                    var nodeIcon = nodeIconGameObject.GetComponent<MapNodeIcon>();
                    nodeIcon.Initiate(node);
                }
            }
        }
    }
}
