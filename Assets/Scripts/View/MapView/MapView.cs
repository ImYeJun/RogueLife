using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Map
{
    public class MapView : ViewBehaviour<IScheduleViewEvent>
    {
        private IReadOnlyDictionary<int, List<Node>> map;
        
        private Dictionary<int, List<MapNodeIcon>> instantiatedNodesByLayer = new Dictionary<int, List<MapNodeIcon>>();
        private List<GameObject> instantiatedLayerObjects = new List<GameObject>(); 

        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject mapLayerViewPrefab;
        [SerializeField] private GameObject mapNodeIconViewPrefab;
        [SerializeField] private MapLineGenerator lineGenerator;

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
            ClearMap();

            DrawNodeIcon();
            
            // 💡 [매우 중요] UI 오브젝트들을 Instantiate한 직후에는 Layout 좌표가 아직 계산되지 않은 상태입니다.
            // 선을 긋기 전에 유니티에게 "지금 당장 UI 좌표들을 전부 계산해서 배치해!" 라고 명령해야 선이 예쁘게 이어집니다.
            Canvas.ForceUpdateCanvases();

            LinkNodeIcon();
        }

        private void ClearMap()
        {
            foreach (var layerObj in instantiatedLayerObjects)
            {
                Destroy(layerObj);
            }
            
            instantiatedLayerObjects.Clear();
            instantiatedNodesByLayer.Clear();
            lineGenerator.ClearLines(); 
        }

        private void DrawNodeIcon()
        {
            var sortedMap = map.OrderBy(pair => pair.Key);

            foreach (var pair in sortedMap)
            {
                int layerIndex = pair.Key;
                var nodes = pair.Value;

                if (nodes.Any(node => node is ScheduleExitNode)) { continue; }

                var layerViewObj = Instantiate(mapLayerViewPrefab, scrollContent);
                instantiatedLayerObjects.Add(layerViewObj);

                var nodeList = new List<MapNodeIcon>();
                
                foreach (var node in nodes)
                {
                    var nodeIconGameObject = Instantiate(mapNodeIconViewPrefab, layerViewObj.transform);
                    var nodeIcon = nodeIconGameObject.GetComponent<MapNodeIcon>();
                    
                    nodeIcon.Initiate(node);
                    nodeList.Add(nodeIcon);
                }

                instantiatedNodesByLayer[layerIndex] = nodeList;
            }
        }

        private void LinkNodeIcon()
        {
            var layers = instantiatedNodesByLayer.Keys.OrderBy(k => k).ToList();

            for (int i = 0; i < layers.Count - 1; i++)
            {
                var currentLayerNodes = instantiatedNodesByLayer[layers[i]];
                var nextLayerNodes = instantiatedNodesByLayer[layers[i + 1]];

                foreach (var nodeIcon in currentLayerNodes)
                {
                    var linkedIcons = nextLayerNodes.Where(nextIcon => 
                        nodeIcon.CurrentNode.NextNodes.Contains(nextIcon.CurrentNode));

                    foreach (var linkedIcon in linkedIcons)
                    {
                        lineGenerator.DrawLine(nodeIcon.transform, linkedIcon.transform);
                    }
                }
            }
        }
    }
}