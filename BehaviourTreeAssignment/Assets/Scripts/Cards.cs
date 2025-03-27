using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : ScriptableObject
{
    [Header("Text")]
    public string title;
    public string description;
    public CardType cardType;

    public int cardCost;
    public int attack;

}

public enum CardType
{
    Attack,
    Defence,
    Heal
}
