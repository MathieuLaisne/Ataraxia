using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.Editor
{
    [System.Serializable]
    public class ListWrapper
    {
        [SerializeField]
        public List<GameObject> myList = new List<GameObject>();
    }
}