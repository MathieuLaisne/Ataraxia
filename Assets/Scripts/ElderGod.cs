using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ataraxia/ElderGod")]
    public class ElderGod : ScriptableObject
    {
        public string name;
        public List<Card> deck;
    }
}