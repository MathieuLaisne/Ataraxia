using Exion.Default;
using UnityEngine;
using UnityEngine.AI;

namespace Exion.Handler
{
    public class CharacterHandler : MonoBehaviour
    {
        public Character character;

        public Vector2 Home;
        public Vector2 Work;

        public int width;

        [SerializeField]
        private Vector3 homePos;
        [SerializeField]
        private Vector3 workPos;

        private Vector3 delta = new Vector3(0.01f, 0.01f, 0);

        NavMeshAgent agent;

        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            homePos = new Vector3((Home.x - width / 2) * 1.2f , (Home.y - width / 2) * 1.2f, 0); 
            workPos = new Vector3((Work.x - width / 2) * 1.2f, (Work.y - width / 2) * 1.2f, 0);
            agent.speed = Random.Range(0.6f, 0.8f);
            
            transform.position = homePos - new Vector3(0,0.2f,0);
            agent.SetDestination(workPos);
        }

        public void Update()
        {
            Roaming();
        }

        private void Roaming()
        {
            if((transform.position - workPos).x < delta.x && (transform.position - workPos).x > -delta.x && (transform.position - workPos).y < delta.y && (transform.position - workPos).y > -delta.y)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
