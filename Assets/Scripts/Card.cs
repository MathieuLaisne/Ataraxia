using UnityEngine;

namespace Exion.Ataraxia.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ataraxia/Card")]
    public class Card : ScriptableObject
    {
        public string name;
        public string description;
        public Sprite image;
    }
}