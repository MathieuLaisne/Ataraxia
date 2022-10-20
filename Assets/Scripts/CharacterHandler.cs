using Exion.Default;
using UnityEngine;
using UnityEngine.AI;

namespace Exion.Handler
{
    public class CharacterHandler : MonoBehaviour
    {
        public Character character;

        private Vector2 home;
        public Vector2 Home
        {
            set
            {
                home = value;
                homePos = new Vector3((home.x - width / 2) * 1.2f + 0.6f, (home.y - width / 2) * 1.2f + 0.6f, 1);
                if (agent == null) agent = GetComponent<NavMeshAgent>();
                NavMeshHit hit;
                NavMesh.SamplePosition(homePos, out hit, 0.5f, NavMesh.AllAreas);
                homePos = hit.position;
                transform.position = homePos;
            }
        }
        private Vector2 work;
        public Vector2 Work
        {
            set 
            {
                work = value;
                workPos = new Vector3((work.x - width / 2) * 1.2f, (work.y - width / 2) * 1.2f, 1);
                if(agent == null) agent = GetComponent<NavMeshAgent>();
                agent.SetDestination(workPos);
                if (!agent.hasPath)
                {
                    NavMeshHit hit;
                    NavMesh.SamplePosition(workPos, out hit, 0.5f, NavMesh.AllAreas);
                    workPos = hit.position;
                }
                agent.SetDestination(workPos);
            }
        }

        public int width;

        [SerializeField]
        private Vector3 homePos;
        [SerializeField]
        private Vector3 workPos;

        NavMeshAgent agent;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = Random.Range(0.6f, 0.8f);
        }

        public void Update()
        {
            Roaming();
        }

        private void Roaming()
        {
            
            if (false)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
