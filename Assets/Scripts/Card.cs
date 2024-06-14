using System;
using UnityEngine;
using Exion.Ataraxia.Default;

namespace Exion.Ataraxia.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ataraxia/Card")]
    public class Card : ScriptableObject
    {
        public string name;
        public string description;
        public Sprite image;
        public Modifier[] Modifiers;
        public Target mainTarget; //which type to highlight
    }
}