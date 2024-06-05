using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.MyEditor
{
    [System.Serializable]
    public class ListWrapper
    {
        [SerializeField]
        public List<GameObject> myList = new List<GameObject>();
    }
}