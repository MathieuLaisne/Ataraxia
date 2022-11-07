using Exion.ScriptableObjects;
using UnityEngine;

namespace Exion.Default
{

    public class Player : MonoBehaviour
    {
        public ElderGod god;
        public Job chosenJob;
        public GameObject firstContact;

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}