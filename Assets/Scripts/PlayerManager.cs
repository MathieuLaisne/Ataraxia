using Exion.Handler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Exion.Default
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI infoName;

        [SerializeField]
        private GameObject info;

        [SerializeField]
        private GameObject characterContainer;

        [SerializeField]
        private GameObject residentList;

        [SerializeField]
        private GameObject workerList;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ray.origin = new Vector3(ray.origin.x, ray.origin.y, -50);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    foreach (Transform t in residentList.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                    foreach (Transform t in workerList.GetComponentInChildren<Transform>()) Destroy(t.gameObject);
                    info.SetActive(true);
                    GameObject objectHit = hit.transform.gameObject;
                    Building BH = objectHit.GetComponent<BuildingHandler>().building;

                    infoName.text = BH.Name;
                    if (BH.Type.hasRes)
                    {
                        foreach (Character c in BH.Residents)
                        {
                            GameObject obj = Instantiate(characterContainer, residentList.transform);
                            obj.GetComponentInChildren<Image>().sprite = c.Profile;
                        }
                    }
                    if (BH.Type.hasWorker)
                    {
                        foreach (Character c in BH.Workers)
                        {
                            GameObject obj = Instantiate(characterContainer, workerList.transform);
                            obj.GetComponentInChildren<Image>().sprite = c.Profile;
                        }
                    }
                }
                else
                {
                    info.SetActive(false);
                }

            }
        }
    }
}