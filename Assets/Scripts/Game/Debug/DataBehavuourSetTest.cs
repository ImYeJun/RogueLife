using System;
using System.Collections.Generic;
using UnityEngine;

public class DataBehavuourSetTest : MonoBehaviour
{
    [Serializable]
    public struct Piar
    {
        [SerializeField] CardData CardData;
        [SerializeReference, SubclassSelector] CardBattleBehaviour behaviour;
    }

    [SerializeField] private List<Piar> piars;
}

