using Exion.Ataraxia.ScriptableObjects;
using UnityEngine;

namespace Exion.Ataraxia.Default
{
    public class Player : MonoBehaviour
    {
        public ElderGod god;
        public Job chosenJob;
        public GameObject firstContact;

        // Start is called before the first frame update
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}