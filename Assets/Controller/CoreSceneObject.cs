using UnityEngine;

namespace Controller.Core
{
    public class CoreSceneObject : MonoBehaviour
    {
        void Start()
        {
            GameSceneManager.Instance.StartGameFlow();
        }
    }
}
