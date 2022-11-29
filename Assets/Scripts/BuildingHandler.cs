using Exion.Ataraxia.Default;
using Exion.Ataraxia.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.Handler
{
    public class BuildingHandler : MonoBehaviour
    {
        public Building building;

        public PlayerManager PM;

        public TimeManager timeManager;

        private List<GameObject> resident = new List<GameObject>();

        public Status jobless;
        public BuildingType abandoned;

        private GameObject structure;

        public void Start()
        {
            PM = FindObjectOfType<PlayerManager>();
            timeManager = FindObjectOfType<TimeManager>();
            structure = transform.Find("Struct").gameObject;
        }

        public void FixedUpdate()
        {
            if (timeManager.Time == "End Work" && building.HasStatus("Trapped")) if(Random.Range(0, 100) < 10) foreach(Character worker in building.Workers) worker.DealHealthDamage(PM.bombCounter * 5);
        }

        public void AddResident(GameObject c)
        {
            resident.Add(c);
        }

        public void DestroyBuilding()
        {
            if(building.Type.destroyable)
            {
                building.Destroy(jobless, abandoned);
                Destroy(structure);
                Instantiate(abandoned.building, transform);
            }
        }
    }
}