using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CosmicHorrorJam/ElderGod")]
public class ElderGod : ScriptableObject
{
    public string name;
    public List<Card> deck;
}
