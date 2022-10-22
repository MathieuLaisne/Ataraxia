using Exion.Default;
using UnityEngine;
using UnityEngine.AI;

namespace Exion.Handler
{
    public class CharacterHandler : MonoBehaviour
    {
        public Character character;

        public PlayerManager PM;

        public Vector2 Home
        {
            set
            {
                if (Random.Range(0, 1) < 0.5) homePos = new Vector3((value.x - width / 2) * 1.2f - 0.6f,
                                                                     (value.y - width / 2) * 1.2f,
                                                                     -1);
                else homePos = new Vector3((value.x - width / 2) * 1.2f,
                                            (value.y - width / 2) * 1.2f - 0.6f,
                                            -1);
                transform.position = homePos;
            }
        }

        public Vector2 Work
        {
            set
            {
                workPos = new Vector3((value.x - width / 2) * 1.2f,
                                        (value.y - width / 2) * 1.2f,
                                        -1);
                if (agent == null) agent = GetComponent<NavMeshAgent>();
                agent.destination = workPos;
            }
        }

        public int width;

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

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            PM = FindObjectOfType<PlayerManager>();
        }

        public void initAll()
        {
            transform.position = homePos;
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            agent.speed = Random.Range(0.3f, 0.5f);
            agent.SetDestination(workPos);
            destinationPos = workPos;
            prevPos = homePos;
        }

        public void Update()
        {
            if(!isPaused) Roaming();
            else agent.isStopped = true;
            if (Input.GetButtonDown("Jump")) isPaused = !isPaused;
        }

        private void Roaming()
        {
            agent.isStopped = false;
            if (!agent.hasPath) recalculatePath();
            else
            {
                if (agent.remainingDistance <= 0.1f)
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
            } else if (other.gameObject.GetComponent<CharacterHandler>())
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
