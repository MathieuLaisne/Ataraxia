using Exion.Handler;
using Exion.ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        [SerializeField]
        private GameObject arrow;

        private GameObject selectedCard;

        private List<GameObject> statusHandlerUI;

        public float suspicion = 0f;
        public Image susBar;
        public TextMeshProUGUI susText;
        public GameObject card;
        public Transform cardContainer;

        public GameObject[] Hand;
        public List<Card> Deck;
        public List<Card> jobDeck;
        public List<Card> Used;
        public List<Card> jobUsed;

        public Player player;

        private GameObject currentArrow;

        [SerializeField]
        private TimeManager tm;

        [SerializeField]
        private GameObject crusader;

        [SerializeField]
        private GameObject gameOver;

        private int nbCrusader = 0;

        public Material[] skyboxes;

        // Start is called before the first frame update
        private void Start()
        {
            RenderSettings.skybox = skyboxes[Random.Range(0, 3)];

            player = FindObjectOfType<Player>();

            statusHandlerUI = new List<GameObject>();
            Hand = new GameObject[3];
            Deck = new List<Card>();
            jobDeck = new List<Card>();
            Used = new List<Card>();
            jobUsed = new List<Card>();

            foreach (Card card in player.god.deck)
            {
                Deck.Add(card);
                Deck.Add(card);
            }

            foreach (Card card in player.chosenJob.deck)
            {
                jobDeck.Add(card);
                jobDeck.Add(card);
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

        private void EmptyHand()
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(Hand[i]);
                Hand[i] = null;
            }
        }

        private void DrawCards()
        {
            if (Deck.Count < 3)
            {
                foreach (Card card in Used)
                {
                    Deck.Add(card);
                }
                Used.Clear();
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

        private void DrawJobHand()
        {
            if(jobDeck.Count < 3)
            {
                foreach(Card card in jobUsed)
                {
                    jobDeck.Add(card);
                }
                jobUsed.Clear();
            }
            Hand[0] = Instantiate(card, cardContainer);
            int rndIdx = Random.Range(0, jobDeck.Count);
            Hand[0].GetComponent<CardHandler>().card = jobDeck[rndIdx];
            jobUsed.Add(jobDeck[rndIdx]);
            jobDeck.RemoveAt(rndIdx);

            rndIdx = Random.Range(0, jobDeck.Count);
            Hand[1] = Instantiate(card, cardContainer);
            Hand[1].GetComponent<CardHandler>().card = jobDeck[Random.Range(0, jobDeck.Count)];
            jobUsed.Add(jobDeck[rndIdx]);
            jobDeck.RemoveAt(rndIdx);

            rndIdx = Random.Range(0, jobDeck.Count);
            Hand[2] = Instantiate(card, cardContainer);
            Hand[2].GetComponent<CardHandler>().card = jobDeck[Random.Range(0, jobDeck.Count)];
            jobUsed.Add(jobDeck[rndIdx]);
            jobDeck.RemoveAt(rndIdx);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                selectedCard.GetComponent<CardHandler>().Highlight(false);
                infoBuilding.SetActive(false);
                infoCharacter.SetActive(false);
                selectedCard = null;
                if (currentArrow) Destroy(currentArrow);
            }
            if (selectedCard && tm.Time != "Night") ApplyGodCard();
            else if(selectedCard) ApplyJobCard();
            else UIDrawer();
        }

        private void FixedUpdate()
        {
            susBar.fillAmount = suspicion / 100;
            susText.text = suspicion.ToString("0.00") + "%";
            if (tm.Time == "End Free")
            {
                EmptyHand();
                DrawJobHand();
            }
            if (tm.Time == "End Night")
            {
                EmptyHand();
                suspicion = Mathf.Clamp(suspicion - 0.05f, 0f, 100f);
                DrawCards();
            }
            if (suspicion >= 25 && nbCrusader <= 0)
            {
                Instantiate(crusader);
                nbCrusader = 1;
            }
            if (suspicion >= 50 && nbCrusader <= 1)
            {
                Instantiate(crusader);
                nbCrusader = 2;
            }
            if (suspicion >= 75 && nbCrusader <= 2)
            {
                Instantiate(crusader);
                nbCrusader = 3;
            }
            if (player.firstContact == null)
            {
                tm.pause = true;
                gameOver.SetActive(true);
            }
        }

        private void ApplyJobCard()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (selectedCard.GetComponent<CardHandler>().card.name)
                {
                    case "Hide Evidence":
                        suspicion -= 5;
                        break;
                    case "Create Drug":
                        jobDeck.Add(RandomDrugCard());
                        break;
                    default:
                        break;
                }
                EmptyHand();
            }
        }

        private Card RandomDrugCard()
        {
            Card RndCard;
            return RndCard
        }

        private void ApplyGodCard()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, new Vector2(0, 0));
            if (Input.GetMouseButtonDown(0))
            {
                switch (selectedCard.GetComponent<CardHandler>().card.name)
                {
                    case "Eerie Land":
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

                    case "Smash Mental Barrier":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                suspicion += CH.DestroyMentalBarrier();

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += CH.DestroyMentalBarrier();

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Force Will":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                suspicion += CH.MakeInsane(CH.Mental * 3);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealMentalDamage(CH.Mental);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += CH.MakeInsane(CH.Mental * 3);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealMentalDamage(CH.Mental);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Press Will":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.DealMentalDamage(5);
                                suspicion += CH.MakeInsane(15);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.DealMentalDamage(5);
                                suspicion += CH.MakeInsane(15);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Wrathful Cry":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                suspicion += CH.MakeInsane(30);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += CH.MakeInsane(30);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Painful Cry":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                suspicion += CH.MakeInsane(45);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealMentalDamage(5);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += CH.MakeInsane(45);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealMentalDamage(5);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Urge":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                suspicion += CH.MakeInsane(30);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealMentalDamage(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += CH.MakeInsane(30);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealMentalDamage(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Take Over":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Taking Over"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Taking Over"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    default:
                        print("Not Implemented yet.");
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
                if (hit2D)
                {
                    if (hit2D.transform.gameObject.GetComponent<CardHandler>())
                    {
                        selectedCard = hit2D.transform.gameObject;
                        hit2D.transform.gameObject.GetComponent<CardHandler>().Highlight(true);
                        infoCharacter.SetActive(false);
                        currentArrow = Instantiate(arrow, selectedCard.transform);
                        currentArrow.transform.localPosition = selectedCard.transform.position;
                    }
                    else if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                    {
                        infoCharacter.SetActive(true);
                        foreach (Transform t in acquaintances.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                        GameObject objectHit = hit2D.transform.gameObject;
                        Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                        infoCharacterName.text = CH.Name;
                        infoCharacterJob.text = CH.Job.name;
                        infoCharacterProfile.sprite = CH.Profile;
                        if (CH.corrupted) infoCharacterProfile.color = new Color(139, 0, 139);
                        else infoCharacterProfile.color = new Color(1, 1, 1);

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
                            CharacterHandlerUI CHUI = obj.GetComponent<CharacterHandlerUI>();
                            CHUI.character = friend;

                            if (CHUI.character.corrupted) CHUI.img.color = new Color(139, 0, 139);
                            else CHUI.img.color = new Color(1, 1, 1);
                        }

                        foreach (GameObject obj in statusHandlerUI) Destroy(obj);
                        foreach (StatusHandler s in CH.Statuses)
                        {
                            GameObject obj = Instantiate(status, characterStatus.transform);
                            statusHandlerUI.Add(obj);
                            obj.GetComponent<StatusHandlerUI>().status = s;
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
                        else infoCharacterProfile.color = new Color(1, 1, 1);

                        if (CH.Barrier > 0)
                        {
                            armor.SetActive(true);
                            armorText.text = CH.Barrier.ToString();
                        }
                        else armor.SetActive(false);

                        hp.fillAmount = (float)CH.HP / (float)CH.maxHP;
                        mp.fillAmount = (float)CH.Mental / (float)CH.maxMental;
                        insanity.fillAmount = (float)CH.Insanity / 100f;

                        HPText.text = CH.HP + "/" + CH.maxHP;
                        MPText.text = CH.Mental + "/" + CH.maxMental;
                        insanityText.text = CH.Insanity + "%";

                        foreach (Character friend in CH.Friends)
                        {
                            GameObject obj = Instantiate(characterContainer, acquaintances.transform);
                            CharacterHandlerUI CHUI = obj.GetComponent<CharacterHandlerUI>();
                            CHUI.character = friend;

                            if (CHUI.character.corrupted) CHUI.img.color = new Color(139, 0, 139);
                            else CHUI.img.color = new Color(1, 1, 1);
                        }

                        foreach (GameObject obj in statusHandlerUI) Destroy(obj);
                        foreach (StatusHandler s in CH.Statuses)
                        {
                            GameObject obj = Instantiate(status, characterStatus.transform);
                            statusHandlerUI.Add(obj);
                            obj.GetComponent<StatusHandlerUI>().status = s;
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
                                CharacterHandlerUI CHUI = obj.GetComponent<CharacterHandlerUI>();
                                CHUI.character = c;

                                if (CHUI.character.corrupted) CHUI.img.color = new Color(139, 0, 139);
                                else CHUI.img.color = new Color(1, 1, 1);
                            }
                        }
                        if (BH.Type.hasWorker)
                        {
                            foreach (Character c in BH.Workers)
                            {
                                GameObject obj = Instantiate(characterContainer, workerList.transform);
                                CharacterHandlerUI CHUI = obj.GetComponent<CharacterHandlerUI>();
                                CHUI.character = c;

                                if (CHUI.character.corrupted) CHUI.img.color = new Color(139, 0, 139);
                                else CHUI.img.color = new Color(1, 1, 1);
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

        public void ReturnMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}