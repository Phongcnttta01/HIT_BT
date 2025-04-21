using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card Data", order = 1)]
public class CardData : ScriptableObject
{
    [SerializeField] public List<GameObject> cards;
    [SerializeField] public List<GameObject> cardHero;
    [SerializeField] public List<GameObject> cardEnemy;
    [SerializeField] public List<GameObject> cardUpdate;
    [SerializeField] public List<GameObject> cardSpecial;
    
    [SerializeField] public List<GameObject> herosList;
}