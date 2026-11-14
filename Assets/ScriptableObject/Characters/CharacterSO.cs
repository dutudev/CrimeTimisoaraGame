using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Character", menuName = "Characters/Character", order=1)]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public List<string> traits = new List<string>();
    public Sprite characterSprite;
    public BuildingInfo located;
    //general info? vector of strings
}