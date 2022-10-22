using System.Collections.Generic;
using UnityEngine;

namespace Exion.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BuildingType", menuName = "CosmicHorrorJam/BuildingType")]
    public class BuildingType : ScriptableObject
    {
        public string m_name;
        public bool hasRes;
        public bool hasWorker;
        public bool destroyable;
        public GameObject building;
        public List<Job> workerType;

        public BuildingType(string name, bool resident, bool workers, bool destroy)
        {
            m_name = name; hasRes = resident; hasWorker = workers; destroyable = destroy;
        }
    }
}