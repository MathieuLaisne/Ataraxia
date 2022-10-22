using Exion.Default;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Handler
{
    public class BuildingHandler : MonoBehaviour
    {
        public Building building;

        [SerializeField]
        private List<GameObject> resident = new List<GameObject>();

        public void Start()
        {
        }

        public void AddResident(GameObject c)
        {
            resident.Add(c);
        }
    }
}