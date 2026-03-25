using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using View.StartMenu;
using ViewEvent.StartMenu;

public class StartMenuManager : MonoBehaviour, IStartMenuViewCommander
{
    private SequenceIdGenerator sequenceIdGenerator = new SequenceIdGenerator();

    [SerializeField] private List<StartDeck> startDecks;
    [SerializeField] private AudioData bgm;
    
    private StartMenuViewEventBus viewEventBus;

    public StartMenuViewEventBus ViewEventBus { get => viewEventBus; }

    public void FixStartDeck(StartDeck startDeck)
    {
        GameRunManager.Instance.StartNewRun(startDeck);

        viewEventBus.Publish(new ReadyToStartGame(sequenceIdGenerator.GetNextId()));
        SoundManager.Instance?.StopBgm();
    }

    public void RequestStartDeckSelect()
    {
        viewEventBus.Publish(new StartDeckLoaded(sequenceIdGenerator.GetNextId(), startDecks));
    }

    private void Awake()
    {
        viewEventBus = new StartMenuViewEventBus();
        SoundManager.Instance?.PlayeBgm(bgm);
    }
}