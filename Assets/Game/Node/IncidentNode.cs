using System.Collections.Generic;
using UnityEngine;

public class IncidentNode : Node
{    
    private List<Choice> choices;

    public List<Choice> Choices { get => choices; }
    
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}