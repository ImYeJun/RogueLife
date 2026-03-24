using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PresentationManager : SingletonManager<PresentationManager> 
{
    private readonly struct QueueItem : IComparable<QueueItem>
    {
        private readonly int sequenceId;
        private readonly int priority;
        private readonly IEnumerator presentation;
        private readonly Action onComplete;

        public QueueItem(int sequenceId, int priority, IEnumerator presentation, Action onComplete = null)
        {
            this.sequenceId = sequenceId;
            this.priority = priority;
            this.presentation = presentation;
            this.onComplete = onComplete;
        }

        public int SequenceId => sequenceId;
        public int Priority => priority;
        public IEnumerator Presentation => presentation;
        public Action OnComplete => onComplete;

        public int CompareTo(QueueItem other)
        {
            int idCompare = sequenceId.CompareTo(other.sequenceId);
            if (idCompare != 0) { return idCompare; }

            return priority.CompareTo(other.priority);
        }
    }

    private bool isPlaying = false;
    private List<QueueItem> queue = new List<QueueItem>();

    public void Enqueue(int sequenceId, int priority, IEnumerator presentation, Action onComplete = null)
    {
        if (presentation == null)
        {
            onComplete?.Invoke();
            return;
        }

        var item = new QueueItem(sequenceId, priority, presentation, onComplete);

        queue.Add(item);
        queue.Sort();

        if (!isPlaying)
        {
            StartCoroutine(PlayQueue());
        }
    }

    private IEnumerator PlayQueue()
    {
        isPlaying = true;

        yield return new WaitForEndOfFrame();

        while (queue.Count > 0)
        {
            var currentSequence = queue[0].SequenceId;
            var currentPriority = queue[0].Priority;
            
            var currentPresentations = new List<QueueItem>();
            
            while (queue.Count > 0 &&
                    queue[0].SequenceId == currentSequence &&
                    queue[0].Priority == currentPriority)
            {
                currentPresentations.Add(queue[0]);
                queue.RemoveAt(0);
            }
            
            yield return StartCoroutine(PlayBatch(currentPresentations));
        }
    
        isPlaying = false;
    }

    private IEnumerator PlayBatch(List<QueueItem> presentations)
    {
        int remainPresentations = presentations.Count;

        foreach (var presentation in presentations)
        {
            StartCoroutine(PlayPresentation(presentation, () => { remainPresentations--; }));
        }

        yield return new WaitUntil(() => remainPresentations <= 0);
    }

    private IEnumerator PlayPresentation(QueueItem presentation, Action onBatchComplete)
    {
        yield return StartCoroutine(presentation.Presentation);

        presentation.OnComplete?.Invoke();
        onBatchComplete.Invoke();
    }

    public void KillAllPresentation()
    {
        StopAllCoroutines();
        DOTween.KillAll();
    }
}