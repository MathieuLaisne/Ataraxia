using Exion.Editor;
using Exion.Handler;
using Exion.ScriptableObjects;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Exion.Default
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private string size;

        [SerializeField]
        private List<BuildingType> buildingTypes;

        [SerializeField]
        private Job[] jobs;

        [SerializeField]
        private GameObject chara;

        public Sprite[] profile;

        public List<string> names;

        private Building[] map;
        private List<Character> npc;

        [SerializeField]
        private List<ListWrapper> charactersMap;

        private List<Character> prisoners = new List<Character>();

        private int nbApp;
        private int nbConstruction;
        private int nbSpecial;
        private int[] jobCount;

        [SerializeField]
        private NavMeshSurface surface;

        [SerializeField]
        private TimeManager timeManager;

        private List<Vector3> parks = new List<Vector3>();

        // Start is called before the first frame update
        private void Start()
        {
            jobCount = new int[jobs.Length];
            npc = new List<Character>();
            charactersMap = new List<ListWrapper>();
            switch (size)
            {
                case "Small":
                    map = new Building[25];
                    BuildMap(25);
                    break;

                default:
                    break;
            }

            CreateResidents();
            SetPrisoner();
            SetAbandoned();
            SetFriends();
            CreateMap();
        }

        private void LateUpdate()
        {
            if(timeManager.Time == "End Work")
            {
                foreach(ListWrapper objs in charactersMap)
                {
                    foreach(GameObject charac in objs.myList)
                    {
                        charac.SetActive(true);
                    }
                }
            }
        }

        private void BuildMap(int size)
        {
            nbApp = size / 2 - 2;
            nbConstruction = size / 15;
            nbSpecial = size / 25;

            for (int i = 0; i < nbApp; i++)
            {
                int pos = 0;
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Appartment", buildingTypes.Find(type => type.name == "Appartment"));
            }

            for (int i = 0; i < nbConstruction; i++)
            {
                int pos = 0;
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Construction Site", buildingTypes.Find(type => type.name == "Construction Site"));
            }
            for (int i = 0; i < nbSpecial; i++)
            {
                int pos = 0;
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Clinic", buildingTypes.Find(type => type.name == "Clinic"));
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Highschool", buildingTypes.Find(type => type.name == "HS"));
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Prison", buildingTypes.Find(type => type.name == "Prison"));
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Police Station", buildingTypes.Find(type => type.name == "Police Station"));
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building("Hospital", buildingTypes.Find(type => type.name == "Hospital"));
            }
            int width = Mathf.RoundToInt(Mathf.Sqrt(size));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (map[i * width + j] == null)
                    {
                        map[i * width + j] = new Building("Park", buildingTypes.Find(type => type.name == "Park"));
                        parks.Add(new Vector3(i * 1.2f - width * 1.2f / 2 + 0.6f, j * 1.2f - width * 1.2f / 2 + 0.6f, -0.1f));
                    }
                }
            }
        }

        private void CreateMap()
        {
            int width = Mathf.FloorToInt(Mathf.Sqrt(map.Length));
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 pos = new Vector3(i * 1.2f - width * 1.2f / 2 + 0.6f, j * 1.2f - width * 1.2f / 2 + 0.6f, -0.1f);
                    var obj = Instantiate(map[i * width + j].Structure, pos, new Quaternion(), transform);
                    BuildingHandler BH = obj.AddComponent<BuildingHandler>();
                    BH.building = map[i * width + j];
                    if (map[i * width + j].Type.hasRes)
                    {
                        foreach (GameObject charact in charactersMap[i * width + j].myList)
                        {
                            charact.GetComponent<CharacterHandler>().Home = new Vector2(i, j);
                            obj.GetComponent<BuildingHandler>().AddResident(charact);
                        }
                    }
                }
            }
            surface.BuildNavMesh();
            foreach (ListWrapper LW in charactersMap)
            {
                foreach(GameObject charact in LW.myList)
                {
                    charact.GetComponent<CharacterHandler>().initAll();
                }
            }
        }

        private void CreateResidents()
        {
            for (int i = 0; i < map.Length * 3.5; i++)
            {
                int rndJob = Random.Range(0, jobs.Length);
                jobCount[rndJob]++;
                Character c = new Character(names[Random.Range(0, names.Count)], 
                                            jobs[rndJob], 
                                            profile[Random.Range(0, profile.Length)], 
                                            Random.Range(15, 30) + Random.Range(15, 30), 
                                            Random.Range(15, 30) + Random.Range(15,30), 
                                            Mathf.Max(Random.Range(-10, 10) + Random.Range(0, 10), 0));
                if (c.Job.name == "Prisoner")
                {
                    prisoners.Add(c);
                }
                else
                {
                    npc.Add(c);
                }
            }
            List<Character> npcCopy = new List<Character>(npc);
            int width = Mathf.FloorToInt(Mathf.Sqrt(map.Length));
            for (int bi = 0; bi < width; bi++)
            {
                for (int bj = 0; bj < width; bj++)
                {
                    charactersMap.Add(new ListWrapper());
                    if (map[bi * width + bj].Type.hasRes && map[bi * width + bj].Type.name != "Prison")
                    {
                        for (int j = 0; j < (npc.Count / (nbApp)); j++)
                        {
                            if (npcCopy.Count <= 0) break;
                            GameObject character = Instantiate(chara, new Vector3(bi * 1.2f - width * 1.2f / 2 + 0.6f, bj * 1.2f - width * 1.2f / 2 + 0.6f, -0.1f), new Quaternion(), transform);
                            character.GetComponent<CharacterHandler>().character = npcCopy[0];
                            character.GetComponent<CharacterHandler>().Home = new Vector3(bi * 1.2f - width * 1.2f / 2 + 0.6f, bj * 1.2f - width * 1.2f / 2 + 0.6f, -0.1f);
                            character.GetComponent<CharacterHandler>().timeManager = timeManager;
                            character.GetComponent<CharacterHandler>().allParks = parks;
                            map[bi * width + bj].AddResident(npcCopy[0]);
                            charactersMap[bi * width + bj].myList.Add(character);
                            npcCopy.Remove(npcCopy[0]);
                        }
                    }
                }
            }
            for (int i = 0; i < jobs.Length; i++)
            {
                jobCount[i] /= 2;
                List<Character> gotJob = new List<Character>();
                List<GameObject> objsCharac = new List<GameObject>();
                foreach (Character c in npc)
                {
                    if (c.Job == jobs[i])
                    {
                        gotJob.Add(c);
                        foreach (ListWrapper lw in charactersMap)
                        {
                            foreach (GameObject charac in lw.myList)
                            {
                                if (c == charac.GetComponent<CharacterHandler>().character) objsCharac.Add(charac);
                            }
                        }
                    }
                }
                int currentIndex = 0;
                if (gotJob.Count > 0)
                {
                    width = Mathf.RoundToInt(Mathf.Sqrt(map.Length));
                    for (int bi = 0; bi < width; bi++)
                    {
                        for (int bj = 0; bj < width; bj++)
                        {
                            if (map[bi * width + bj].Type.hasWorker)
                            {
                                if (map[bi * width + bj].CanBeWorkedBy(jobs[i]))
                                {
                                    int j;
                                    for (j = currentIndex; j < currentIndex + jobCount[i] / nbSpecial; j++)
                                    {
                                        if (j >= objsCharac.Count) break;
                                        map[bi * width + bj].AddWorker(gotJob[j]);
                                        objsCharac[j].GetComponent<CharacterHandler>().Work = new Vector3(bi * 1.2f - width * 1.2f / 2 + 0.6f, bj * 1.2f - width * 1.2f / 2 + 0.6f, -0.1f);
                                    }
                                    currentIndex = j;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetAbandoned()
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].Type.hasRes && map[i].Residents.Count == 0)
                {
                    map[i] = new Building(buildingTypes[0].m_name, buildingTypes[0]);
                }
            }
        }

        public void SetPrisoner()
        {
            int nbPrisoner = 0;
            foreach (Building b in map)
            {
                if (b.Type.name == "Prison")
                {
                    int i;
                    for (i = nbPrisoner; i < nbPrisoner + prisoners.Count / nbSpecial; i++)
                    {
                        b.AddResident(prisoners[i]);
                    }
                    nbPrisoner = i;
                }
            }
        }

        public void SetFriends()
        {
            foreach (Character c in npc)
            {
                int friendMax = Random.Range(2, npc.Count / 10);
                while (c.Friends.Count < friendMax)
                {
                    int rndIndex = Random.Range(0, npc.Count);
                    while (npc[rndIndex] == c)
                    {
                        rndIndex = Random.Range(0, npc.Count);
                    }
                    c.AddFriend(npc[rndIndex]);
                    npc[rndIndex].AddFriend(c);
                }
            }
            foreach (Character c in prisoners)
            {
                int friendMax = Random.Range(2, prisoners.Count / 10);
                while (c.Friends.Count < friendMax)
                {
                    int rndIndex = Random.Range(0, prisoners.Count);
                    while (prisoners[rndIndex] == c)
                    {
                        rndIndex = Random.Range(0, prisoners.Count);
                    }
                    c.AddFriend(prisoners[rndIndex]);
                    prisoners[rndIndex].AddFriend(c);
                }
            }
        }
    }
}