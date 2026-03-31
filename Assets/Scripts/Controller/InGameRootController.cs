using System.Collections;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InGameRootController : SceneRootController{
    protected GameRun currentRun;
    protected System.Random random;

    protected override void OnStart()
    {
        currentRun = GameRunManager.Instance?.CurrentRun;

        if (currentRun is null)
        {
            Debug.LogWarning("[SceneRootController] There's no GameRunManager or GameRun. Creating a empty Run");
            currentRun = GameRunManager.Instance?.GetEmptyRun();
        }

        random = currentRun.Random;

        OnInitialize();
    }
}