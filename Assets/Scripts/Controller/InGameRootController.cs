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
            Debug.LogError("[SceneRootController] There's no GameRunManager or GameRun.");

            return;
        }

        random = currentRun.Random;

        OnInitialize();
    }
}