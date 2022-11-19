using Exion.Ataraxia.Default;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.Handler
{
    public class BuildingHandler : MonoBehaviour
    {
        public Building building;

        public PlayerManager PM;

        public TimeManager timeManager;

        [SerializeField]
        private List<GameObject> resident = new List<GameObject>();

        public void Start()
        {
            PM = FindObjectOfType<PlayerManager>();
            timeManager = FindObjectOfType<TimeManager>();
        }

        public void FixedUpdate()
        {
            if (timeManager.Time == "End Work" && building.HasStatus("Trapped"))
            {
            }
        }

        public void AddResident(GameObject c)
        {
            resident.Add(c);
        }
    }
}