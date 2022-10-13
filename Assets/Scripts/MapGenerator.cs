using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private string size;

    [SerializeField]
    private BuildingType[] buildingTypes;

    [SerializeField]
    private Job[] jobs;

    public List<string> names;

    private Building[] map;
    private List<Character> npc;

    private int nbApp;
    private int nbConstruction;
    private int nbAbandoned;
    private int nbSpecial;
    private int[] jobCount;

    // Start is called before the first frame update
    void Start()
    {
        jobCount = new int[jobs.Length];
        npc = new List<Character>();
        switch(size) {
            case "Small":
                    map = new Building[25];
                    BuildMap(25);
                break;
            default:
                break;
        }
        
        CreateResidents();
        CreateMap();
    }

    private void BuildMap(int size) {
        nbApp = size / 2 - 2;
        nbConstruction = size / 15;
        nbAbandoned = size / 25;
        nbSpecial = size / 25;
        Debug.Log("app " + nbApp + " construc " + nbConstruction + " aband " + nbAbandoned + " spec " + nbSpecial);

        for(int i = 0; i < nbApp; i++)
        {
            int pos = 0;
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[1]);
        }

        for(int i = 0; i < nbConstruction; i++)
        {
            int pos = 0;
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[3]);
        }

        for(int i = 0; i < nbAbandoned; i++)
        {
            int pos = 0;
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[0]);
        }

        for(int i = 0; i < nbSpecial; i++)
        {
            int pos = 0;
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[2]);
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[4]);
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[5]);
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[7]);
            do {
                pos = Random.Range(0, size);
            } while(map[pos] != null);
            map[pos] = new Building("", buildingTypes[8]);
        }
        
        for(int i = 0; i < size; i++)
        {
            if(map[i] == null)
            {
                map[i] = new Building("", buildingTypes[6]);
            }
        }
    }

    private void CreateMap() {
        int width = Mathf.FloorToInt(Mathf.Sqrt(map.Length));
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < width; j++)
            {
                Vector3 pos = new Vector3(i*1.1f, j*1.1f, 0);
                var obj = GameObject.Instantiate(map[i*width + j].Structure, pos, new Quaternion());
                obj.AddComponent<BuildingHandler>();
                obj.GetComponent<BuildingHandler>().building = map[i*j+width];
            }
        }
    }

    private void CreateResidents() {
        for(int i = 0; i < map.Length * 2; i++) {
            int rndJob = Random.Range(0, jobs.Length);
            jobCount[rndJob]++;
            Character c = new Character(names[Random.Range(0, names.Count)], jobs[rndJob], Random.Range(10, 30), Random.Range(10, 30), Random.Range(0, 10));
            npc.Add(c);
        }
        int currIndex = 0;
        foreach(Building building in map) {
            if(building.Type.hasRes) {
                for(int i = currIndex; i < npc.Count / nbApp; i++)
                {
                    building.AddResident(npc[i]);
                }
            }
        }
        for(int i = 0; i < jobs.Length; i++)
        {
            jobCount[i] /= 2;
            List<Character> gotJob = new List<Character>();
            foreach(Character c in npc)
            {
                if(c.Job == jobs[i]) {
                    gotJob.Add(c);
                }
            }
            int currentIndex = 0;
            foreach(Building b in map)
            {
                if(b.Type.hasWorker)
                {
                    if(b.CanBeWorkedBy(jobs[i])) {
                        for(int j = currentIndex; j < jobCount[i]; j++)
                        {
                            b.AddWorker(gotJob[j]);
                        }
                    }
                }
            }
        }
    }
}
