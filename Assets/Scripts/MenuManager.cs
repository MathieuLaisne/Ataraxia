using Exion.Default;
using Exion.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exion.Handler
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        public GameObject godPanel;
        public GameObject jobPanel;

        public void SelectGod(ElderGod god)
        {
            player.god = god;
        }

        public void SelectFirstContact(Job job)
        {
            player.chosenJob = job;
        }

        public void NewGame()
        {
            godPanel.SetActive(true);
        }

        public void ValidateGod()
        {
            jobPanel.SetActive(true);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}