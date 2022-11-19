using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ataraxia/Job")]
    public class Job : ScriptableObject
    {
        public string name;
        public List<Card> deck;
    }
}