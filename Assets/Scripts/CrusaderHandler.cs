using UnityEngine;
using UnityEngine.AI;

namespace Exion.Ataraxia.Handler
{
    public class CrusaderHandler : MonoBehaviour
    {
        private NavMeshAgent agent;
        public TimeManager timeManager;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            timeManager = FindObjectOfType<TimeManager>();
        }

        public void Update()
        {
            if (!timeManager.pause) agent.isStopped = false;
            else agent.isStopped = true;
        }

        public void FixedUpdate()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 0.6f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 0.6f, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<CharacterHandler>())
            {
                if (other.gameObject.GetComponent<CharacterHandler>().character.corrupted && Random.Range(0, 100) > 75) Destroy(other.gameObject);
            }
        }
    }
}