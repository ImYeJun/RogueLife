using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace View.BattleView
{
    public class BattleViewTransitionManager : MonoBehaviour
    {
        public enum ViewType
        {
            Main, 
            EntityInspect
        }

        [Serializable]
        private struct ViewSet : IEquatable<ViewSet>
        {
            public ViewType viewType;
            public CinemachineVirtualCamera camera;
            public CanvasGroup canvasGroup;

            public bool Equals(ViewSet other)
            {
                return viewType == other.viewType && 
                    camera == other.camera && 
                    canvasGroup == other.canvasGroup;
            }
        }

        [SerializeField] private BattleEntityInspectorView entityInspectorView;
        [SerializeField] private Vector3 entityInspectOffset;

        [Header("View Configurations")]
        [SerializeField] private List<ViewSet> viewConfigurations;

        private Dictionary<ViewType, ViewSet> viewDict = new Dictionary<ViewType, ViewSet>();
        private ViewType currentViewType;

        private void Awake()
        {
            foreach (var viewSet in viewConfigurations)
            {
                if (!viewDict.ContainsKey(viewSet.viewType))
                {
                    viewDict.Add(viewSet.viewType, viewSet);
                }
            }

            SwitchView(viewDict[ViewType.Main]);
        }

        public void InspectEntity(IInspectable inspectable, Transform entityPosition, BattleEntityInspectorView.InspectorDirection direction)
        {
            if (viewDict.TryGetValue(ViewType.EntityInspect, out ViewSet targetSet))
            {
                var camera = targetSet.camera;
                camera.Follow = entityPosition;
                
                var transposer = camera.GetCinemachineComponent<CinemachineTransposer>();
                
                float originalZ = transposer.m_FollowOffset.z;
                var offset = direction switch
                {
                    BattleEntityInspectorView.InspectorDirection.Left => new Vector3(-entityInspectOffset.x, entityInspectOffset.y, originalZ),
                    BattleEntityInspectorView.InspectorDirection.Right => new Vector3(entityInspectOffset.x, entityInspectOffset.y, originalZ),
                    _ => throw new InvalidOperationException($"[BattleViewTransitionManager] {direction} is invalid.")
                };

                transposer.m_FollowOffset = offset;
                camera.PreviousStateIsValid = false;
                
                SwitchView(targetSet);
            }

            entityInspectorView.InspectEntity(inspectable, direction);
        }

        public void ReturnToMain()
        {
            if (viewDict.TryGetValue(ViewType.Main, out ViewSet targetSet))
            {
                SwitchView(targetSet);
            }
        }

        private void SwitchView(ViewSet targetViewSet)
        {            
            foreach (ViewSet currentViewSet in viewDict.Values)
            {
                bool isTarget = currentViewSet.Equals(targetViewSet);
                
                if (currentViewSet.canvasGroup != null)
                {
                    SetActiveCanvas(currentViewSet.canvasGroup, isTarget);
                }

                if (currentViewSet.camera != null)
                {
                    currentViewSet.camera.Priority = isTarget ? 10 : 0;
                }
            }

            currentViewType = targetViewSet.viewType;
        }

        private void SetActiveCanvas(CanvasGroup canvas, bool value)
        {
            canvas.alpha = value ? 1 : 0;
            canvas.interactable = value;
            canvas.blocksRaycasts = value;
        }
    }
}