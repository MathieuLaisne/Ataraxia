using Exion.Default;
using UnityEngine;
using UnityEngine.AI;

namespace Exion.Handler
{
    public class CharacterHandler : MonoBehaviour
    {
        [SerializeField]
        public Character character;

        public Vector2 Home
        {
            set
            {
                if (Random.Range(0,1) < 0.5) homePos = new Vector3( (value.x - width / 2) * 1.2f - 0.6f,
                                                                    (value.y - width / 2) * 1.2f, 
                                                                    -1);
                else homePos = new Vector3( (value.x - width / 2) * 1.2f,
                                            (value.y - width / 2) * 1.2f - 0.6f, 
                                            -1);
                transform.position = homePos;
            }
        }

        public Vector2 Work
        {
            set 
            {
                workPos = new Vector3(  (value.x - width / 2) * 1.2f, 
                                        (value.y - width / 2) * 1.2f, 
                                        -1);
                if (agent == null) agent = GetComponent<NavMeshAgent>();
                agent.destination = workPos;
            }
        }

        public int width;

        [SerializeField]
        private Vector3 homePos;
        [SerializeField]
        private Vector3 workPos;
        [SerializeField]
        private Vector3 destinationPos;

        NavMeshAgent agent;

        Vector3 prevPos;
        int stuckFrames = 0;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void initAll()
        {
            transform.position = homePos;
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            agent.speed = Random.Range(0.3f, 0.5f);
            agent.SetDestination(workPos);
            prevPos = homePos;
        }

        public void Update()
        {
            Roaming();
        }

        private void Roaming()
        {
            destinationPos = workPos;
            if (!agent.hasPath) recalculatePath();
            else
            {
                if (agent.remainingDistance <= 0.1f)
                {
                    gameObject.SetActive(false);
                } else
                {
                    if (prevPos == transform.position) stuckFrames++;
                    else stuckFrames = 0;
                    if (stuckFrames > 60) recalculatePath();
                    prevPos = transform.position;
                }
            }
        }

        private void recalculatePath() {
            NavMeshHit hit;
            NavMesh.SamplePosition(destinationPos, out hit, 0.6f, NavMesh.AllAreas);
            destinationPos = hit.position;
            agent.SetDestination(destinationPos);
        }
    }
}
