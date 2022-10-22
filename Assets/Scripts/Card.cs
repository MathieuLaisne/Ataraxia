using UnityEngine;

namespace Exion.ScriptableObjects
{
    [CreateAssetMenu(menuName = "CosmicHorrorJam/Card")]
    public class Card : ScriptableObject
    {
        public string name;
        public string description;
        public Sprite image;
    }
}