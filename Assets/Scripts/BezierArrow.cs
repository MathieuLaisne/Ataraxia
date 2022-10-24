using System.Collections.Generic;
using UnityEngine;

public class BezierArrow : MonoBehaviour
{
    public GameObject ArrowHead;
    public GameObject ArrowNode;
    public int nodeNum;
    public float scale = 1f;

    private List<RectTransform> arrowNodes = new List<RectTransform>();
    private List<Vector2> controlPoints = new List<Vector2>();
    private readonly List<Vector2> controlPointsFactor = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < nodeNum; i++) arrowNodes.Add(Instantiate(ArrowNode, transform).GetComponent<RectTransform>());

        arrowNodes.Add(Instantiate(ArrowHead, transform).GetComponent<RectTransform>());

        arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1, 1));

        for (int i = 0; i < 4; i++) controlPoints.Add(new Vector2());
    }

    // Update is called once per frame
    void Update()
    {
        controlPoints[0] = new Vector2(transform.position.x, transform.position.y);

        controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointsFactor[0];
        controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointsFactor[1];

        for(int i = 0; i < arrowNodes.Count; i++)
        {
            float t = Mathf.Log(1f * i / (arrowNodes.Count - 1) + 1f, 2f);

            arrowNodes[i].position =    Mathf.Pow(1 - t, 3) * controlPoints[0] +
                                        3 * Mathf.Pow(1 - t, 2) * controlPoints[1] +
                                        3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                                        Mathf.Pow(t, 3) * controlPoints[3];

            if(i > 0)
            {
                Vector3 euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, arrowNodes[i].position - arrowNodes[i - 1].position));
                arrowNodes[i].rotation = Quaternion.Euler(euler);
            }

            float lScale = scale * (1f - 0.03f * (arrowNodes.Count - 1 - i));
            arrowNodes[i].localScale = new Vector3(lScale, lScale, 1);

        }

        arrowNodes[0].transform.rotation = arrowNodes[1].transform.rotation;
    }
}
