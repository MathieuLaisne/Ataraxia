using Exion.Handler;
using Exion.ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UIDrawer();
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
                    if (hit.transform.gameObject.GetComponent<BuildingHandler>())
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
                        foreach(StatusHandler s in BH.Statuses)
                        {
                            GameObject obj = Instantiate(status, buildingStatus.transform);
                            obj.GetComponent<StatusHandlerUI>().status = s;
                        }
                    }
                    else if (hit.transform.gameObject.GetComponent<CharacterHandler>())
                    {
                        infoCharacter.SetActive(true);
                        infoBuilding.SetActive(false);
                        foreach (Transform t in acquaintances.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                        GameObject objectHit = hit.transform.gameObject;
                        Character CH = objectHit.GetComponent<CharacterHandler>().character;

                        infoCharacterName.text = CH.Name;
                        infoCharacterJob.text = CH.Job.name;
                        infoCharacterProfile.sprite = CH.Profile;

                        foreach(Character friend in CH.Friends)
                        {
                            GameObject obj = Instantiate(characterContainer, acquaintances.transform);
                            obj.GetComponent<CharacterHandlerUI>().character = friend;
                        }
                    }
                } else if (hit2D)
                {
                    infoCharacter.SetActive(true);
                    foreach (Transform t in acquaintances.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                    GameObject objectHit = hit2D.transform.gameObject;
                    Character CH = objectHit.GetComponent<CharacterHandlerUI>().character;

                    infoCharacterName.text = CH.Name;
                    infoCharacterJob.text = CH.Job.name;
                    infoCharacterProfile.sprite = CH.Profile;

                    foreach (Character friend in CH.Friends)
                    {
                        GameObject obj = Instantiate(characterContainer, acquaintances.transform);
                        obj.GetComponent<CharacterHandlerUI>().character = friend;
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