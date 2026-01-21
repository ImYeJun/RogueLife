using System.Collections.Generic;
using UnityEngine;

public class TradeNode : Node
{
    private List<Choice> choices;

    public List<Choice> Choices { get => choices; }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO : choices에 따라 선택지 UI 띄우기
    }
}