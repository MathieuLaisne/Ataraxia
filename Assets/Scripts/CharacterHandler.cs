using Exion.Ataraxia.Default;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Exion.Ataraxia.Handler
{
    public class CharacterHandler : MonoBehaviour
    {
        public Character character;

        public PlayerManager PM;

        public TimeManager timeManager;

        public List<Vector3> allParks = new List<Vector3>();

        public Vector3 Home
        {
            set
            {
                homePos = value;
            }
        }

        public Vector3 Work
        {
            set
            {
                workPos = value;
            }
        }

        public int width;

        [SerializeField]
        private Vector3 homePos;

        [SerializeField]
        private Vector3 workPos;

        [SerializeField]
        private Vector3 destinationPos;

        private NavMeshAgent agent;

        private Vector3 prevPos;

        [SerializeField]
        private int stuckFrames = 0;

        public bool isPaused = true;

        [SerializeField]
        private Vector3 todayFree;

        [SerializeField]
        private Material material;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            PM = FindObjectOfType<PlayerManager>();
        }

        public void initAll()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            NavMeshHit hit;
            if (NavMesh.SamplePosition(homePos, out hit, 0.6f, NavMesh.AllAreas)) homePos = hit.position;
            transform.localPosition = homePos;
            agent.speed = Random.Range(0.3f, 0.5f);
            agent.SetDestination(workPos);
            destinationPos = workPos;
            prevPos = homePos;
            material = GetComponent<MeshRenderer>().material;
        }

        public void Update()
        {
            isPaused = timeManager.pause;
            if (!isPaused) Roaming();
            else agent.isStopped = true;
        }

        public void FixedUpdate()
        {
            if (character.corrupted) material.color = new Color(139, 0, 139);
            else material.color = new Color();
            if (timeManager.Time == "End Work") todayFree = allParks[Random.Range(0, allParks.Count)];
            if (timeManager.Time == "End Night" && character.HasStatus("Taking Over")) character.DealMentalDamage(2);
            if (timeManager.Time == "Night" && character.Job.name == "Student") GoToRave();
        }

        private void GoToRave()
        {
        }

        private void Roaming()
        {
            agent.isStopped = false;
            if (!agent.hasPath)
            {
                switch (timeManager.Time)
                {
                    case "Free Time":
                        destinationPos = todayFree;
                        recalculatePath();
                        break;

                    case "End Free":
                        destinationPos = homePos;
                        recalculatePath();
                        break;

                    case "Morning":
                        destinationPos = workPos;
                        recalculatePath();
                        if (agent.remainingDistance <= 0.2f)
                        {
                            gameObject.SetActive(false);
                        }
                        break;

                    default:
                        if (agent.remainingDistance <= 0.2f)
                        {
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            if (prevPos == transform.position) stuckFrames++;
                            else stuckFrames = 0;
                            if (stuckFrames > 60) recalculatePath();
                            prevPos = transform.position;
                        }
                        break;
                }
            }
        }

        private void recalculatePath()
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(destinationPos, out hit, 0.6f, NavMesh.AllAreas);
            destinationPos = hit.position;
            agent.SetDestination(destinationPos);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<BuildingHandler>())
            {
                BuildingHandler BH = other.gameObject.GetComponent<BuildingHandler>();
                foreach (StatusHandler status in BH.building.Statuses)
                {
                    switch (status.status.name)
                    {
                        case "Eerie":
                            character.DealMentalDamage(status.stacks);
                            PM.suspicion += status.stacks * 0.1f;
                            break;

                        default:
                            break;
                    }
                }
            }
            else if (other.gameObject.GetComponent<CharacterHandler>())
            {
                CharacterHandler CH = other.gameObject.GetComponent<CharacterHandler>();
                foreach (StatusHandler status in CH.character.Statuses)
                {
                    switch (status.status.name)
                    {
                        case "Confusion":
                            if (Random.Range(0, 1) < 0.5)
                            {
                                if (character.DealHealthDamage(CH.character.Strength))
                                {
                                    Destroy(this);
                                }
                                PM.suspicion += 0.5f;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}