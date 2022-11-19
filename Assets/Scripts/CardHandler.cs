using Exion.Ataraxia.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Exion.Ataraxia.Handler
{
    public class CardHandler : MonoBehaviour
    {
        public Card card;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardText;
        public Image cardImg;
        public GameObject highlight;

        // Start is called before the first frame update
        private void Start()
        {
            cardName.text = card.name;
            cardText.text = card.description;
            cardImg.sprite = card.image;
        }

        public void Highlight(bool state)
        {
            if (state) transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            else transform.localScale = new Vector3(1, 1, 1);
            highlight.SetActive(state);
        }
    }
}