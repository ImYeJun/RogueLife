using System;
using UnityEngine;

namespace View.ScheduleView.TransactionNodeView
{
    public class TransactionChoiceButton : DoubleTextSelectButton
    {
        public void Initiate(TransactionChoiceData choiceData, Action onPressed)
        {
            Unactive();

            // DoubleTextSelectButton의 Initialize 호출 (메인 + 서브)
            Initialize(onPressed, choiceData.Description, choiceData.SubDescription);
            
            gameObject.SetActive(true);
        }

        public void Unactive()
        {
            gameObject.SetActive(false);
        }
    }
}