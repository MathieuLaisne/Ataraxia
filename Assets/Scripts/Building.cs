using Exion.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Default
{
    public class Building
    {
        private string m_name;
        public string Name
        {
            get { return m_name; }
        }
        private BuildingType m_type;
        public BuildingType Type
        {
            get { return m_type; }
        }

        private List<string> m_status;
        public List<string> Statuses
        {
            get { return m_status; }
        }

        [SerializeField]
        private List<Character> m_residents;
        public List<Character> Residents
        {
            get { return m_residents; }
        }

        private List<Character> m_workers;
        public List<Character> Workers
        {
            get { return m_workers; }
        }

        private GameObject m_building;
        public GameObject Structure
        {
            get { return m_building; }
        }

        public Building(string name, BuildingType type)
        {
            m_name = name; m_type = type;
            m_status = new List<string>();
            if (type.hasRes) m_residents = new List<Character>();
            if (type.hasWorker) m_workers = new List<Character>();
            m_building = type.building;
        }

        public void Destroy()
        {
            if (m_type.destroyable)
            {
                if (m_type.hasWorker)
                {
                    foreach (Character worker in m_workers)
                    {
                        worker.ApplyStatus("Jobless");
                    }
                }
                m_type = new BuildingType("Construction Site", false, true, false);
                m_workers = new List<Character>();
            }
        }

        public void ApplyStatus(string name)
        {
            m_status.Add(name);
        }

        public void AddResident(Character c)
        {
            m_residents.Add(c);
        }

        public void AddWorker(Character c)
        {
            m_workers.Add(c);
        }

        public bool CanBeWorkedBy(Job job)
        {
            foreach (Job j in m_type.workerType)
            {
                if (j == job) return true;
            }
            return false;
        }
    }
}
