using Exion.Ataraxia.Handler;
using Exion.Ataraxia.ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Exion.Ataraxia.Default
{
    public class PlayerManager : MonoBehaviour
    {
        #region UI
        [SerializeField]
        private GameObject tutorial;

        #region Building UI
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
        #endregion

        #region Character UI
        [SerializeField]
        private GameObject characterContainer;

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
        #endregion

        [SerializeField]
        private GameObject status;

        #region HP UI
        [SerializeField]
        private Image hp;
        [SerializeField]
        private TextMeshProUGUI HPText;
        #endregion

        #region MP UI
        [SerializeField]
        private TextMeshProUGUI MPText;
        [SerializeField]
        private Image mp;

        [SerializeField]
        private GameObject armor;
        [SerializeField]
        private TextMeshProUGUI armorText;
        #endregion

        #region Insanity UI
        [SerializeField]
        private Image insanity;
        [SerializeField]
        private TextMeshProUGUI insanityText;
        #endregion

        #region card UI
        [SerializeField]
        private GameObject arrow;
        private GameObject currentArrow;
        #endregion

        private List<GameObject> statusHandlerUI;

        [SerializeField]
        private GameObject gameOver;

        public Material[] skyboxes;
        #endregion

        #region suspicion
        public float suspicion = 0f;
        public Image susBar;
        public TextMeshProUGUI susText;
        #endregion

        #region cards
        public GameObject card;
        public Transform cardContainer;

        public GameObject[] Hand;

        public List<Card> Deck;
        public List<Card> jobDeck;

        public List<Card> Used;
        public List<Card> jobUsed;

        [SerializeField]
        private Card[] Drugs;

        private GameObject selectedCard;

        [SerializeField]
        private List<Status> AllStatus;
        #endregion

        public Player player;
        [SerializeField]
        private TimeManager tm;

        #region crusader
        [SerializeField]
        private GameObject crusader;

        private int nbCrusader = 0;
        #endregion

        public int bombCounter = 1;

        // Start is called before the first frame update
        private void Start()
        {
            RenderSettings.skybox = skyboxes[Random.Range(0, 3)];

            player = FindObjectOfType<Player>();

            if (PlayerPrefs.GetInt("Tutorial") == 1) ShowTutorial();

            switch (player.god.name)
            {
                case "Cthulhu":
                    if (PlayerPrefs.GetInt("Played Ruthless") == 0)
                    {
                        ShowGod(player.god);
                    }
                    break;

                case "Nyarlatothep":
                    if (PlayerPrefs.GetInt("Played Deceiver") == 0)
                    {
                        ShowGod(player.god);
                    }
                    break;

                case "Shub-Niggurath":
                    if (PlayerPrefs.GetInt("Played Taintress") == 0)
                    {
                        ShowGod(player.god);
                    }
                    break;

                default:
                    break;
            }

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
            if (jobDeck.Count < 3)
            {
                foreach (Card card in jobUsed)
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
            else if (selectedCard) ApplyJobCard();
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
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, new Vector2(0, 0));
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

                    case "Mental Drug":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.DealMentalDamage(5);
                                suspicion -= 2;

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.DealMentalDamage(5);
                                suspicion -= 2;

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Weakening Drug":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.DealHealthDamage(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.DealHealthDamage(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Stimulant":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.Heal(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.Heal(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Plan Rave":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Building BH = objectHit.GetComponent<BuildingHandler>().building;

                                if (BH.Name == "Abandoned Building")
                                {
                                    BH.ApplyStatus(AllStatus.Find(s => s.name == "Rave"), 1);

                                    selectedCard.GetComponent<CardHandler>().Highlight(false);
                                    Destroy(selectedCard);
                                    selectedCard = null;
                                }
                                else
                                {
                                    print("Invalid building");
                                }
                            }
                        }
                        break;

                    case "Trap Building":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Building BH = objectHit.GetComponent<BuildingHandler>().building;

                                BH.ApplyStatus(AllStatus.Find(s => s.name == "Trapped"), bombCounter);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Bomb":
                        bombCounter++;
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
            RndCard = Drugs[Random.Range(0, Drugs.Length)];
            return RndCard;
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
                    #region Taintress Cards
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

                    case "Destroy Building":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Building BH = objectHit.GetComponent<BuildingHandler>().building;

                                if (BH.Type.name == "Abandonned Building") print("wrong building");
                                else
                                {
                                    BH.Destroy();

                                    selectedCard.GetComponent<CardHandler>().Highlight(false);
                                    Destroy(selectedCard);
                                    selectedCard = null;
                                }

                            }
                        }
                        break;

                    case "Horrifying Land":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Building BH = objectHit.GetComponent<BuildingHandler>().building;

                                BH.ApplyStatus(AllStatus.Find(s => s.name == "Horrifying"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Horror's Nest":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Building BH = objectHit.GetComponent<BuildingHandler>().building;

                                BH.ApplyStatus(AllStatus.Find(s => s.name == "Nest"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Stigmata":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Stigmatized"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Stigmatized"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;
                    case "Fleshwarp":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.Fleshwarp(10);
                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Fleshwarped"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.Fleshwarp(10);
                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Fleshwarped"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Hidden Fleshwarp":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.Fleshwarp(5);
                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Unremarkable Fleshwarp"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.Fleshwarp(5);
                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Unremarkable Fleshwarp"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;
                    #endregion

                    #region Ruthless Cards
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
                                GameObject objectHit = hit2D.transform.gameObject;
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
                                CH.DealDirectMentalDamage(CH.Mental);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += CH.MakeInsane(CH.Mental * 3);
                                if (CH.Insanity == 100) CH.ApplyStatus(AllStatus.Find(s => s.name == "Confusion"), 1);
                                CH.DealDirectMentalDamage(CH.Mental);

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
                                GameObject objectHit = hit2D.transform.gameObject;
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
                                GameObject objectHit = hit2D.transform.gameObject;
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
                                GameObject objectHit = hit2D.transform.gameObject;
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
                                GameObject objectHit = hit2D.transform.gameObject;
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
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Taking Over"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;
                    #endregion

                    #region Deceiver Cards
                    case "Daunting Emotion":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                suspicion += 0.5f * CH.DealMentalDamage(10);
                                if (Random.Range(0, 100) < 50) CH.ApplyStatus(AllStatus.Find(s => s.name == "Nightmare"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                suspicion += 0.5f * CH.DealMentalDamage(10);
                                if (Random.Range(0, 100) < 50) CH.ApplyStatus(AllStatus.Find(s => s.name == "Nightmare"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;
                    case "Echoing Idea":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                if (CH.DealMentalDamage(7) == 0) CH.DealMentalDamage(7);
                                
                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                if (CH.DealMentalDamage(7) == 0) CH.DealMentalDamage(7);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Fake Memory":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.DealDirectMentalDamage(15);
                                suspicion += 0.5f * CH.Friends.Count;

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.DealDirectMentalDamage(15);
                                suspicion += 0.5f * CH.Friends.Count;

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);
                                selectedCard = null;
                            }
                        }
                        break;

                    case "Labyrinthine Mind":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                if (!CH.DealDirectMentalDamage(10))
                                {
                                    selectedCard.GetComponent<CardHandler>().Highlight(false);
                                    Destroy(selectedCard);
                                }

                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                if (!CH.DealDirectMentalDamage(10))
                                {
                                    selectedCard.GetComponent<CardHandler>().Highlight(false);
                                    Destroy(selectedCard);
                                }

                                selectedCard = null;
                            }
                        }
                        break;

                    case "Mental Stab":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                if (CH.DealMentalDamage(7) == 0) CH.DealMentalDamage(7);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);

                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                if (CH.DealMentalDamage(10) > 0) CH.MakeInsane(10);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);

                                selectedCard = null;
                            }
                        }
                        break;

                    case "Plant The Seed":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Seeded"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);

                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.ApplyStatus(AllStatus.Find(s => s.name == "Seeded"), 1);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);

                                selectedCard = null;
                            }
                        }
                        break;

                    case "Whisper":
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                            {
                                GameObject objectHit = hit.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandler>().character;

                                CH.DealDirectMentalDamage(7);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);

                                selectedCard = null;
                            }
                        }
                        else if (hit2D)
                        {
                            if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                            {
                                GameObject objectHit = hit2D.transform.gameObject;
                                Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                                CH.DealDirectMentalDamage(7);

                                selectedCard.GetComponent<CardHandler>().Highlight(false);
                                Destroy(selectedCard);

                                selectedCard = null;
                            }
                        }
                        break;
                    #endregion
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
                        currentArrow = Instantiate(arrow, selectedCard.transform);
                        currentArrow.transform.position = selectedCard.transform.position + new Vector3(0, 100, 0);
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

        private void ShowTutorial()
        {
            tm.pause = true;
            tutorial.SetActive(true);
        }

        private void ShowGod(ElderGod god)
        {
            tm.pause = true;
            switch (god.name)
            {
                case "Cthulhu":
                    PlayerPrefs.SetInt("Played Ruthless", 1);
                    break;

                case "Nyarlatothep":
                    PlayerPrefs.SetInt("Played Deceiver", 1);
                    break;

                case "Shub-Niggurath":
                    PlayerPrefs.SetInt("Played Taintress", 1);
                    break;

                default:
                    break;
            }
        }
    }
}