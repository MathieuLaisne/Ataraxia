using Exion.Default;
using UnityEngine;
using UnityEngine.AI;

namespace Exion.Handler
{
    public class CharacterHandler : MonoBehaviour
    {
        public Character character;

        public int Home;
        public int Work;

        public int width;

        private Vector3 homePos;
        private Vector3 workPos;

        NavMeshAgent agent;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            homePos = new Vector3(Mathf.Floor(Home / width) * 1.1f - width * 1.1f / 2 + 0.6f, Home % width - width * 1.1f / 2 + 0.6f, 0);
            workPos = new Vector3(Mathf.Floor(Work / width) * 1.1f - width * 1.1f / 2 + 0.6f, Work % width - width * 1.1f / 2 + 0.6f, 0);
            agent.speed = Random.Range(0.05f, 0.2f);
            agent.destination = workPos;
            transform.position = homePos;
        }

        public void Update()
        {
            Roaming();
        }

        private void Roaming()
        {
        }
    }
}
