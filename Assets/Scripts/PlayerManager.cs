using Exion.Handler;
using Exion.ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Exion.Default
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI infoBuildingName;

        [SerializeField]
        private GameObject residentList;

        [SerializeField]
        private GameObject infoBuilding;

        [SerializeField]
        private GameObject workerList;

        [SerializeField]
        private GameObject buildingStatus;

        [SerializeField]
        private TextMeshProUGUI infoCharacterName;

        [SerializeField]
        private TextMeshProUGUI infoCharacterJob;

        [SerializeField]
        private Image infoCharacterProfile;

        [SerializeField]
        private GameObject infoCharacter;

        [SerializeField]
        private GameObject acquaintances;

        [SerializeField]
        private GameObject characterStatus;

        [SerializeField]
        private GameObject status;

        [SerializeField]
        private GameObject characterContainer;

        [SerializeField]
        private List<Status> AllStatus;

        [SerializeField]
        private GameObject armor;

        [SerializeField]
        private Image hp;

        [SerializeField]
        private Image mp;

        [SerializeField]
        private Image insanity;

        [SerializeField]
        private TextMeshProUGUI armorText;

        [SerializeField]
        private TextMeshProUGUI HPText;

        [SerializeField]
        private TextMeshProUGUI MPText;

        [SerializeField]
        private TextMeshProUGUI insanityText;

        private GameObject selectedCard;

        private List<GameObject> statusHandlerUI;

        public float suspicion = 0f;
        public Image susBar;
        public TextMeshProUGUI susText;
        public GameObject card;
        public Transform cardContainer;

        public GameObject[] Hand;
        public List<Card> Deck;
        public List<Card> Used;

        public ElderGod player;

        // Start is called before the first frame update
        private void Start()
        {
            statusHandlerUI = new List<GameObject>();
            Hand = new GameObject[3];
            Deck = new List<Card>();
            Used = new List<Card>();
            foreach(Card card in player.deck)
            {
                Deck.Add(card);
                Deck.Add(card);
            }
            Hand[0] = Instantiate(card, cardContainer);
            int rndIdx = Random.Range(0, Deck.Count);
            Hand[0].GetComponent<CardHandler>().card = Deck[rndIdx];
            Used.Add(Deck[rndIdx]);
            Deck.RemoveAt(rndIdx);

            rndIdx = Random.Range(0, Deck.Count);
            Hand[1] = Instantiate(card, cardContainer);
            Hand[1].GetComponent<CardHandler>().card = Deck[Random.Range(0, Deck.Count)];
            Used.Add(Deck[rndIdx]);
            Deck.RemoveAt(rndIdx);

            rndIdx = Random.Range(0, Deck.Count);
            Hand[2] = Instantiate(card, cardContainer);
            Hand[2].GetComponent<CardHandler>().card = Deck[Random.Range(0, Deck.Count)];
            Used.Add(Deck[rndIdx]);
            Deck.RemoveAt(rndIdx);
        }

        // Update is called once per frame
        private void Update()
        {
            if (selectedCard) ApplyCard();
            else UIDrawer();
        }

        private void LateUpdate()
        {
            susBar.fillAmount = suspicion / 100;
            susText.text = suspicion.ToString("0.00") + "%";
        }

        private void ApplyCard()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (selectedCard.GetComponent<CardHandler>().card.name)
                {
                    case "Eerie Land":
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        ray.origin = new Vector3(ray.origin.x, ray.origin.y, -50);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Building BH = objectHit.GetComponent<BuildingHandler>().building;
                                BH.ApplyStatus(AllStatus.Find(s => s.name == "Eerie"), 1);
                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void UIDrawer()
        {
            RaycastHit hit;
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, new Vector2(0, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ray.origin = new Vector3(ray.origin.x, ray.origin.y, -50);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit2D.transform.gameObject.GetComponent<CardHandler>())
                    {
                        selectedCard = hit2D.transform.gameObject;
                        hit2D.transform.gameObject.GetComponent<CardHandler>().Highlight(true);
                    }
                    else if(hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                    {
                        infoCharacter.SetActive(true);
                        foreach (Transform t in acquaintances.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                        GameObject objectHit = hit2D.transform.gameObject;
                        Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                        infoCharacterName.text = CH.Name;
                        infoCharacterJob.text = CH.Job.name;
                        infoCharacterProfile.sprite = CH.Profile;
                        if (CH.corrupted) infoCharacterProfile.color = new Color(139, 0, 139);

                        if (CH.Barrier > 0)
                        {
                            armor.SetActive(true);
                            armorText.text = CH.Barrier.ToString();
                        }
                        else armor.SetActive(false);

                        hp.fillAmount = (float)CH.HP / (float)CH.maxHP;
                        mp.fillAmount = (float)CH.Mental / (float)CH.maxMental;
                        insanity.fillAmount = (float)CH.Insanity / 100;

                        HPText.text = CH.HP + "/" + CH.maxHP;
                        MPText.text = CH.Mental + "/" + CH.maxMental;
                        insanityText.text = CH.Insanity + "%";

                        foreach (Character friend in CH.Friends)
                        {
                            GameObject obj = Instantiate(characterContainer, acquaintances.transform);
                            obj.GetComponent<CharacterHandlerUI>().character = friend;
                        }
                    }
                }
                else if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                    {
                        infoCharacter.SetActive(true);
                        infoBuilding.SetActive(false);
                        foreach (Transform t in acquaintances.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                        GameObject objectHit = hit.transform.gameObject;
                        Character CH = objectHit.GetComponent<CharacterHandler>().character;

                        infoCharacterName.text = CH.Name;
                        infoCharacterJob.text = CH.Job.name;
                        infoCharacterProfile.sprite = CH.Profile;
                        if (CH.corrupted) infoCharacterProfile.color = new Color(139, 0, 139);

                        if (CH.Barrier > 0)
                        {
                            armor.SetActive(true);
                            armorText.text = CH.Barrier.ToString();
                        }
                        else armor.SetActive(false);

                        hp.fillAmount = CH.HP / CH.maxHP;
                        mp.fillAmount = CH.Mental / CH.maxMental;
                        insanity.fillAmount = CH.Insanity / 100;

                        HPText.text = CH.HP + "/" + CH.maxHP;
                        MPText.text = CH.Mental + "/" + CH.maxMental;
                        insanityText.text = CH.Insanity + "%";

                        foreach (Character friend in CH.Friends)
                        {
                            GameObject obj = Instantiate(characterContainer, acquaintances.transform);
                            obj.GetComponent<CharacterHandlerUI>().character = friend;
                        }
                    }
                    else if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                    {
                        infoCharacter.SetActive(false);
                        foreach (Transform t in residentList.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                        foreach (Transform t in workerList.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                        infoBuilding.SetActive(true);
                        GameObject objectHit = hit.transform.gameObject;
                        Building BH = objectHit.GetComponent<BuildingHandler>().building;

                        infoBuildingName.text = BH.Name;
                        if (BH.Type.hasRes)
                        {
                            foreach (Character c in BH.Residents)
                            {
                                GameObject obj = Instantiate(characterContainer, residentList.transform);
                                obj.GetComponent<CharacterHandlerUI>().character = c;
                            }
                        }
                        if (BH.Type.hasWorker)
                        {
                            foreach (Character c in BH.Workers)
                            {
                                GameObject obj = Instantiate(characterContainer, workerList.transform);
                                obj.GetComponent<CharacterHandlerUI>().character = c;
                            }
                        }
                        foreach (GameObject obj in statusHandlerUI) Destroy(obj);
                        statusHandlerUI = new List<GameObject>();
                        foreach (StatusHandler s in BH.Statuses)
                        {
                            GameObject obj = Instantiate(status, buildingStatus.transform); 
                            statusHandlerUI.Add(obj);
                            obj.GetComponent<StatusHandlerUI>().status = s;
                        }
                    }
                    
                }
                else
                {
                    infoBuilding.SetActive(false);
                    infoCharacter.SetActive(false);
                }
            }
        }
    }
}