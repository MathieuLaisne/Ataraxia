using Exion.Ataraxia.Default;
using UnityEngine;
using UnityEngine.UI;

namespace Exion.Ataraxia.Handler
{
    public class CharacterHandlerUI : MonoBehaviour
    {
        public Character character;
        public Image img;

        public void Start()
        {
            img.sprite = character.Profile;
            if (character.corrupted) img.color = new Color(139, 0, 139);
        }
    }
}