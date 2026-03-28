using System;
using UnityEngine;

namespace View.ScheduleView.IncidentNodeView
{
    public class IncidentChoiceButton : SingleTextSelectButton 
    {
        public void Initiate(DeterminedIncidentChoice choice, Action onPressed)
        {
            Unactive();

            // 기존의 순서 보장(Unactive -> Invoke)을 위해 람다로 묶어 부모에게 전달
            Action wrappedAction = () => 
            {
                Unactive();
                onPressed?.Invoke();
            };

            // SingleTextSelectButton의 Initialize 호출
            Initialize(wrappedAction, choice.Description);
            
            gameObject.SetActive(true);
        }

        public void Unactive()
        {
            gameObject.SetActive(false);
        }
    }
}