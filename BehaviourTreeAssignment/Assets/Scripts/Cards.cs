using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Cards")]
public class Cards : ScriptableObject
{
    [Header("Text")]
    public int cardID;
    public string title;
    public string description;
    public int priorityType;
}
