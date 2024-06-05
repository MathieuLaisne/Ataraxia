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
            if (timeManager.Time == "End Work")
            {
                if (building.HasStatus("Trapped"))
                {
                    building.DamageWorkers(building.AmountStatus("Trapped"));
                    building.DamageResidents(building.AmountStatus("Trapped"));
                }
                if (building.AmountStatus("Trapped") >= 5)
                {
                    building.Destroy();
                }
            }
            if (timeManager.Time == "End Night" && building.HasStatus("Rave"))
            {
                building.RemoveStatus("Rave");
            }
        }

        public void AddResident(GameObject c)
        {
            resident.Add(c);
        }
    }
}