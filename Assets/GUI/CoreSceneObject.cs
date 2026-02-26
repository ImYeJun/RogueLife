using UnityEngine;

public class CoreSceneObject : MonoBehaviour
{
    void Start()
    {
        GameSceneManager.Instance.StartGameFlow();
    }
}
