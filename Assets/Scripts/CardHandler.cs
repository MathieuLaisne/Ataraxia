using Exion.Ataraxia.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Exion.Ataraxia.Handler
{
    public class CardHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        public Card card;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardText;
        public Image cardImg;
        public GameObject highlight;

        public GameObject statusList;
        public GameObject tooltip;

        private List<GameObject> listStatus;

        // Start is called before the first frame update
        private void Start()
        {
            cardName.text = card.name;
            cardText.text = card.description;
            cardImg.sprite = card.image;
            listStatus = new List<GameObject>();
        }

        public void Highlight(bool state)
        {
            if (state) transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            else transform.localScale = new Vector3(1, 1, 1);
            highlight.SetActive(state);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach(GameObject obj in listStatus)
            {
                Destroy(obj);
            }
            listStatus = new List<GameObject>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (Status s in card.inflictedStatus)
            {
                GameObject obj = Instantiate(tooltip, statusList.transform);
                GameObject.Find(obj.name+"/name").GetComponent<TextMeshProUGUI>().text = s.name;
                GameObject.Find(obj.name+"/desc").GetComponent<TextMeshProUGUI>().text = s.description;
                listStatus.Add(obj);
            }
        }
    }
}