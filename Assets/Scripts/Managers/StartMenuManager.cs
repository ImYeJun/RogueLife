using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using View.StartMenu;
using ViewEvent.StartMenu;

public class StartMenuManager : MonoBehaviour, IStartMenuViewCommander
{
    [SerializeField] private List<StartDeck> startDecks;
    [SerializeField] private GameObject startDeckSelectPanel;
    
    private StartMenuViewEventBus viewEventBus;

    public StartMenuViewEventBus ViewEventBus { get => viewEventBus; }

    public void FixStartDeck(StartDeck startDeck)
    {
        GameRunManager.Instance.StartNewRun(startDeck);

        viewEventBus.Publish(new ReadyToStartGame());
    }

    public void RequestStartDeckSelect()
    {
        viewEventBus.Publish(new StartDeckLoaded(startDecks));

        startDeckSelectPanel.SetActive(true);
    }

    private void Awake()
    {
        startDeckSelectPanel.SetActive(false);
        viewEventBus = new StartMenuViewEventBus();
    }
}