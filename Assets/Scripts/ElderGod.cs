using System.Collections.Generic;
using UnityEngine;

namespace Exion.ScriptableObjects
{
    [CreateAssetMenu(menuName = "CosmicHorrorJam/ElderGod")]
    public class ElderGod : ScriptableObject
    {
        public string name;
        public List<Card> deck;
    }
}