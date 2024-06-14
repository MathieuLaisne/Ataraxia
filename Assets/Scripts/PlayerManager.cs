using Exion.Ataraxia.Handler;
using Exion.Ataraxia.ScriptableObjects;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Random = UnityEngine.Random;
using UnityEditor;

namespace Exion.Ataraxia.Default
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject tutorial;

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

        [SerializeField]
        private Card[] Drugs;

        public Material[] skyboxes;

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
                CardHandler CardH = selectedCard.GetComponent<CardHandler>();
                CardH.Highlight(false);
                HighlightCorrespondingTarget(false, CardH.card.mainTarget);
                infoBuilding.SetActive(false);
                infoCharacter.SetActive(false);
                selectedCard = null;
                if (currentArrow) Destroy(currentArrow);
            }
            /*if (selectedCard && tm.Time != "Night") ApplyGodCard();
            else if (selectedCard) ApplyJobCard();*/
            if (selectedCard) ApplyCardEffect();
            else UIDrawer();
        }

        private Character GetClickedCharacter(Ray ray, RaycastHit2D hit2D)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                {
                    GameObject objectHit = hit.transform.gameObject;
                    Character CH = objectHit.GetComponent<CharacterHandler>().character;

                    return CH;
                }
            }
            else if (hit2D)
            {
                if (hit2D.transform.gameObject.GetComponent<CharacterHandlerUI>())
                {
                    GameObject objectHit = hit2D.transform.gameObject;
                    Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                    return CH;
                }
            }
            return null;
        }

        private BuildingHandler GetClickedBuilding(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<BuildingHandler>())
                {
                    GameObject objectHit = hit.transform.gameObject;
                    BuildingHandler BH = objectHit.GetComponent<BuildingHandler>();

                    return BH;
                }
            }
            return null;
        }

        private void ApplyCardEffect()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, new Vector2(0, 0));

            Card c = selectedCard.GetComponent<CardHandler>().card;

            Character CH = GetClickedCharacter(ray, hit2D);

            BuildingHandler BH = GetClickedBuilding(ray);

            if (Input.GetMouseButtonDown(0))
            {
                bool effectApplied = false;
                bool emptyHand = false;
                for (int i = 0; i < c.Modifiers.Length; i++)
                {
                    if(ModifierHandler.ApplyModifier(c.Modifiers, c.Modifiers[i], CH, BH, ref suspicion, ref bombCounter, ref jobDeck, RandomDrugCard(), out emptyHand))
                        effectApplied = true;
                }
                if(emptyHand)
                {
                    CardHandler CardH = selectedCard.GetComponent<CardHandler>();
                    CardH.Highlight(false);
                    HighlightCorrespondingTarget(false, CardH.card.mainTarget);
                    Destroy(selectedCard);
                    selectedCard = null;
                    EmptyHand();
                }
                if (effectApplied)
                {
                    CardHandler CardH = selectedCard.GetComponent<CardHandler>();
                    CardH.Highlight(false);
                    HighlightCorrespondingTarget(false, CardH.card.mainTarget);
                    Destroy(selectedCard);
                    selectedCard = null;
                }
            }
        }

        private void FixedUpdate()
        {
            susBar.fillAmount = suspicion / 100;
            susText.text = suspicion.ToString("0.00") + "%";
            if (tm.Time == TimePeriod.End_Free_Time)
            {
                EmptyHand();
                DrawJobHand();
            }
            if (tm.Time == TimePeriod.End_Night)
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

        private Card RandomDrugCard()
        {
            Card RndCard;
            RndCard = Drugs[Random.Range(0, Drugs.Length)];
            return RndCard;
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
                        HighlightCorrespondingTarget(true, selectedCard.GetComponent<CardHandler>().card.mainTarget);

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

        private void HighlightCorrespondingTarget(bool active, Target target)
        {
            switch (target)
            {
                case Target.HUMANS:
                    foreach(GameObject selection in GameObject.FindGameObjectsWithTag("Human"))
                    {
                        selection.GetComponent<Outline>().enabled = active;
                    }
                    break;
                case Target.MONSTER:

                    foreach (GameObject selection in GameObject.FindGameObjectsWithTag("Monster"))
                    {
                        selection.GetComponent<Outline>().enabled = active;
                    }

                    //TODO Make a Monster class and tag
                    break;
                case Target.BUILDING:
                    foreach (GameObject selection in GameObject.FindGameObjectsWithTag("Building"))
                    {
                        selection.GetComponent<Outline>().enabled = active;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}