using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.ScriptableObjects
{
    [CreateAssetMenu(menuName = "CosmicHorrorJam/Status")]
    public class Status : ScriptableObject
    {
        public string name;
        public string description;

        public Status(string n, string d)
        {
            name = n; description = d;
        }
    }
}