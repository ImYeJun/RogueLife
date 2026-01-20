using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : Component
{
    public T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance != null && Instance != this){
            Destroy(this);
        }
        else {
            Instance = this as T;
            DontDestroyOnLoad(Instance);
        }
    }
}
