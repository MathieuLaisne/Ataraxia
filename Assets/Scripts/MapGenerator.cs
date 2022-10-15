using Exion.Editor;
using Exion.Handler;
using Exion.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Default
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private string size;

        [SerializeField]
        private BuildingType[] buildingTypes;

        [SerializeField]
        private Job[] jobs;

        [SerializeField]
        private GameObject chara;

        public Sprite[] profile;

        public List<string> names;

        private Building[] map;
        private List<Character> npc;

        [SerializeField]
        private List<ListWrapper> characters;
        private List<Character> prisoners = new List<Character>();

        private int nbApp;
        private int nbConstruction;
        private int nbAbandoned;
        private int nbSpecial;
        private int[] jobCount;

        // Start is called before the first frame update
        private void Start()
        {
            jobCount = new int[jobs.Length];
            npc = new List<Character>();
            characters = new List<ListWrapper>();
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
            CreateMap();
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
                map[pos] = new Building(buildingTypes[1].m_name, buildingTypes[1]);
            }

            for (int i = 0; i < nbConstruction; i++)
            {
                int pos = 0;
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building(buildingTypes[3].m_name, buildingTypes[3]);
            }
            for (int i = 0; i < nbSpecial; i++)
            {
                int pos = 0;
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building(buildingTypes[2].m_name, buildingTypes[2]);
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building(buildingTypes[4].m_name, buildingTypes[4]);
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building(buildingTypes[5].m_name, buildingTypes[5]);
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building(buildingTypes[7].m_name, buildingTypes[7]);
                do
                {
                    pos = Random.Range(0, size);
                } while (map[pos] != null);
                map[pos] = new Building(buildingTypes[8].m_name, buildingTypes[8]);
            }

            for (int i = 0; i < size; i++)
            {
                if (map[i] == null)
                {
                    map[i] = new Building(buildingTypes[6].m_name, buildingTypes[6]);
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
                    Vector3 pos = new Vector3(i * 1.1f - width * 1.1f / 2, j * 1.1f - width * 1.1f / 2, 0);
                    var obj = Instantiate(map[i * width + j].Structure, pos, new Quaternion());
                    BuildingHandler BH = obj.AddComponent<BuildingHandler>();
                    BH.building = map[i * width + j];
                    if (map[i * width + j].Type.hasRes)
                    {
                        foreach (GameObject charact in characters[i * width + j].myList)
                        {
                            obj.GetComponent<BuildingHandler>().AddResident(charact);
                        }
                    }
                }
            }
        }

        private void CreateResidents()
        {
            for (int i = 0; i < map.Length * 3.5; i++)
            {
                int rndJob = Random.Range(0, jobs.Length);
                jobCount[rndJob]++;
                Character c = new Character(names[Random.Range(0, names.Count)], jobs[rndJob], profile[Random.Range(0, profile.Length)], Random.Range(10, 30), Random.Range(10, 30), Random.Range(0, 10));
                if(c.Job.name == "Prisoner")
                {
                    prisoners.Add(c);
                } else
                {
                    npc.Add(c);
                }
            }
            List<Character> npcCopy = new List<Character>(npc);
            for (int i = 0; i < map.Length; i++)
            {
                characters.Add(new ListWrapper());
                if (map[i].Type.hasRes)
                {
                    for (int j = 0; j < (npc.Count / (nbApp)); j++)
                    {
                        if (npcCopy.Count <= 0) break;
                        GameObject character = Instantiate(chara);
                        character.AddComponent<CharacterHandler>();
                        character.GetComponent<CharacterHandler>().character = npcCopy[0];
                        map[i].AddResident(npcCopy[0]);
                        characters[i].myList.Add(character);
                        npcCopy.Remove(npcCopy[0]);
                    }
                }
            }
            for (int i = 0; i < jobs.Length; i++)
            {
                jobCount[i] /= 2;
                List<Character> gotJob = new List<Character>();
                foreach (Character c in npc)
                {
                    if (c.Job == jobs[i])
                    {
                        gotJob.Add(c);
                    }
                }
                int currentIndex = 0;
                if(gotJob.Count > 0)
                {
                    foreach (Building b in map)
                    {
                        if (b.Type.hasWorker)
                        {
                            if (b.CanBeWorkedBy(jobs[i]))
                            {
                                int j;
                                for (j = currentIndex; j < currentIndex + jobCount[i] / nbSpecial; j++)
                                {
                                    b.AddWorker(gotJob[j]);
                                }
                                currentIndex = j;
                            }
                        }
                    }
                }
            }
        }

        public void SetAbandoned()
        {
            for(int i = 0; i < map.Length; i++)
            {
                if(map[i].Type.hasRes && map[i].Residents.Count == 0)
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
                if(b.Type.name == "Prison")
                {
                    int i;
                    for(i = nbPrisoner; i < nbPrisoner + prisoners.Count / nbSpecial; i++)
                    {
                        b.AddResident(prisoners[i]);
                    }
                    nbPrisoner = i;
                }
            }
        }
    }
}