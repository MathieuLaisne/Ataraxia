using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CosmicHorrorJam/Job")]
public class Job : ScriptableObject
{
    public string name;
    public List<Card> deck;


}
