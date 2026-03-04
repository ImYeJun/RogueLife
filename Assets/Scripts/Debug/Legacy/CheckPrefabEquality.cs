using System.Runtime.Remoting.Contexts;
using UnityEngine;

public class CheckPrefabEquality : MonoBehaviour
{
    [SerializeField] private DataBehavuourSetTest set;
    [SerializeField] private GameObject prefab;

    [ContextMenu("Check Equality")]
    public void CheckEquality()
    {
        Debug.Log(set.Equals(prefab.GetComponent<DataBehavuourSetTest>()));
    }
}
