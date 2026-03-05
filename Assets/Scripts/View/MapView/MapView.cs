using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Map
{
    public class MapView : ViewBehaviour<IScheduleViewEvent>
    {
        private IReadOnlyDictionary<int, List<Node>> map;
        
        private Dictionary<int, List<MapNodeIcon>> instantiatedNodesByLayer = new Dictionary<int, List<MapNodeIcon>>();
        private Dictionary<Node, MapNodeIcon> nodeIconMap = new Dictionary<Node, MapNodeIcon>();
        
        private MapNodeIcon currentIconView;
        private Node targetNode;
        
        private List<GameObject> instantiatedLayerObjects = new List<GameObject>(); 

        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject mapLayerViewPrefab;
        [SerializeField] private GameObject mapNodeIconViewPrefab;
        [SerializeField] private Scrollbar scrollbar;
        [SerializeField] private MapLineGenerator lineGenerator;

        private bool isMapDirty = false;

        public override void OnInitialized()
        {
            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<NodeMoved>(OnNodeMoved);
        }
        
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<NodeMoved>(OnNodeMoved);
        }

        private void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            map = payload.Schedule.Map;
            
            isMapDirty = true;
            if (gameObject.activeInHierarchy)
            {
                DrawMap();
            }
        }
        
        private void OnNodeMoved(NodeMoved payload)
        {
            targetNode = payload.CurrentNode;

            ApplyNodeFocus();   
        }

        private void ApplyNodeFocus()
        {
            if (targetNode == null) return;

            if (nodeIconMap.TryGetValue(targetNode, out MapNodeIcon iconView))
            {
                currentIconView?.OnUnfocused();
                currentIconView = iconView;
                currentIconView.OnFocused();
            }
            else
            {
                Debug.LogWarning("[MapView] Model map and View map are not matched.");
            }
        }

        public void OnViewOpened()
        {
            gameObject.SetActive(true);
            
            if (isMapDirty)
            {
                DrawMap();
            }

            FocusOnCurrentNode();
        }

        private void FocusOnCurrentNode()
        {
            if (currentIconView == null || scrollbar == null || map == null || map.Count == 0) return;

            int currentLayer = -1;
            foreach (var pair in map)
            {
                if (pair.Value.Contains(currentIconView.CurrentNode))
                {
                    currentLayer = pair.Key;
                    break;
                }
            }

            if (currentLayer == -1) return;

            int minLayer = map.Keys.Min();
            int maxLayer = map.Keys.Max();

            float normalizedValue = 0f;
            if (maxLayer > minLayer)
            {
                normalizedValue = (float)(currentLayer - minLayer) / (maxLayer - minLayer);
            }

            if (scrollbar.direction == Scrollbar.Direction.TopToBottom || scrollbar.direction == Scrollbar.Direction.RightToLeft)
            {
                normalizedValue = 1f - normalizedValue;
            }

            scrollbar.value = Mathf.Clamp01(normalizedValue);
        }

        private void DrawMap()
        {
            ClearMap();

            DrawNodeIcon();
            Canvas.ForceUpdateCanvases();
            LinkNodeIcon();

            isMapDirty = false;

            ApplyNodeFocus();
        }

        private void ClearMap()
        {
            foreach (var layerObj in instantiatedLayerObjects)
            {
                Destroy(layerObj);
            }
            
            instantiatedLayerObjects.Clear();
            instantiatedNodesByLayer.Clear();
            
            nodeIconMap.Clear(); 
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

                    nodeIconMap.Add(node, nodeIcon);
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