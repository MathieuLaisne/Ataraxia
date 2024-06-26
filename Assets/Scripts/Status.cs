using Exion.Ataraxia.Default;
using UnityEngine;

namespace Exion.Ataraxia.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ataraxia/Status")]
    public class Status : ScriptableObject
    {
        public string name;
        public string description;
        public Modifier[] Modifiers;

        public Status(string n, string d)
        {
            name = n; description = d;
        }
    }
}