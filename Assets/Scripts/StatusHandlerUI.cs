using TMPro;
using UnityEngine;

namespace Exion.Ataraxia.Handler
{
    public class StatusHandlerUI : MonoBehaviour
    {
        public StatusHandler status;
        public TextMeshProUGUI statusName;
        public GameObject tooltip;
        public TextMeshProUGUI statusDescription;

        public void Init()
        {
            statusName.text = status.status.name;
            if (status.stacks > 1) statusName.text += " x" + status.stacks;
            statusDescription.text = status.status.description;
        }

        private void OnMouseEnter()
        {
            tooltip.SetActive(true);
        }
        private void OnMouseExit()
        {
            tooltip.SetActive(false);
        }
    }
}