using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager m_instance;
    public TextMeshProUGUI description;

    private void Awake()
    {
        if(m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetTooltip(string desc)
    {
        gameObject.SetActive(true);
        description.text = desc;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        description.text = "";
    }
}
