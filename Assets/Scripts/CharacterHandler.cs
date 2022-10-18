using Exion.Default;
using UnityEngine;

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

        private float speed;

        public void Start()
        {
            homePos = new Vector3(Mathf.Floor(Home / width) * 1.1f - width * 1.1f / 2, Home % width - width * 1.1f / 2, 0);
            workPos = new Vector3(Mathf.Floor(Work / width) * 1.1f - width * 1.1f / 2, Work % width - width * 1.1f / 2, 0);
            speed = Random.Range(0.05f, 0.2f);
            transform.position = homePos;
        }

        public void Update()
        {
            Roaming();
        }

        private void Roaming()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, workPos, step);
        }
    }
}
