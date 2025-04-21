using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardBoxManager : MonoBehaviour
{
    [SerializeField] private CardManager cm;
    [SerializeField] private SelectManager slm;

    public void ButtonRemove()
    {
        if (GameManager.Instance.isCanClick)
        {
            cm.RemoveAllChoices();
        }
    }

    public void ButtonAllow()
    {
        if (GameManager.Instance.isCanClick)
        {
            List<int> choices = new List<int>();
            choices = cm.Allow();
            cm.RemoveAllChoices();
            foreach (var choice in choices)
            {
                if (choice == 1)
                {
                    slm.SpawnChoices1();
                }

                if (choice == 2)
                {
                    slm.SpawnChoices2();
                }
            }
        }
    }
}
