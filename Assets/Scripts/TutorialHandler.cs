using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Exion.Ataraxia.Handler
{
    public class TutorialHandler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI tutorial;

        [SerializeField]
        private TextMeshProUGUI next;

        [SerializeField]
        private GameObject previous;

        private int step;

        private void Start()
        {
            step = 0;
        }

        public void PressNext()
        {
            step++;
            TextShow();
        }

        public void PressPrev()
        {
            step--;
            Mathf.Clamp(step, 0, 10);
            TextShow();
        }

        private void TextShow()
        {
            switch (step)
            {
                case 0:
                    tutorial.text = "Hello!\n" +
                        "It seems it is the first time you are playing <color=purple>Ataraxia</color>, or maybe did you want to see the tutorial again?\n" +
                        "Let me explain to you a bit about this game.";
                    previous.SetActive(false);
                    break;

                case 1:
                    tutorial.text = "<color=purple>Ataraxia</color> is a Strategy Card Game requiring you, an Eldritch God, to corrupt the minds of mortals living in this city.\n" +
                        "Your first contact will help you in doing so, but beware of not raising <color=#4B00AB>Suspicion</color> too fast, or you might bring Crusaders on you.";
                    previous.SetActive(true);
                    break;

                case 2:
                    tutorial.text = "During the day, you can play all your drawn cards on the corresponding targets: Buildings, or Humans.\n" +
                        "Each Human has a workplace and a residence. They will go back and forth between those places during the day, so plan ahead.\n" +
                        "They also have acquaintances that might be suspicious of your doings.";
                    break;

                case 3:
                    tutorial.text = "To see a building's details, you can just click on it.\n" +
                        "There, you'll see who works there and who lives along with the current status affecting it.\n" +
                        "Clicking on a Human from this menu will bring you their status";
                    break;

                case 4:
                    tutorial.text = "On Human status, you'll see their <color=#AB0000>Health</color>," +
                        "their <color=#000EAB>Mental Strength</color> and their <color=#AB005C>Insanity</color>.\n" +
                        "You'll also notice what is their work and who are their friends.\n" +
                        "It is also possible to click on the friends to bring their status or to click on a passerby for the same result.";
                    break;

                case 5:
                    tutorial.text = "During the night, you'll draw 3 cards corresponding to your first contact's job.\n" +
                        "You can only play one per night, so choose wisely.\n" +
                        "Furthermore, as mere mortals, they won't be able to help much most of the time, doing menial things like Laundry or Partying.";
                    break;

                default:
                    tutorial.text = "Finally, as the powerful entity that you are, you can control time to a certain degree.\n" +
                        "As such, pressing space will let you pause or unpause time, so that you have time planning your next move.";
                    next.text = "Finish";
                    PlayerPrefs.Save();
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void selectTuto(Toggle toggle)
        {
            int value = toggle.isOn == true ? 1 : 0;
            PlayerPrefs.SetInt("Tutorial", value);
        }
    }
}