using System.Collections.Generic;
using UnityEngine;

namespace Exion.ScriptableObjects
{
    [CreateAssetMenu(menuName = "CosmicHorrorJam/Job")]
    public class Job : ScriptableObject
    {
        public string name;
        public List<Card> deck;
    }
}