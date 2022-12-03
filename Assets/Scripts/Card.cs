using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ataraxia/Card")]
    public class Card : ScriptableObject
    {
        public string name;
        [Multiline]
        public string description;
        public Sprite image;

        public List<Status> inflictedStatus;
    }
}