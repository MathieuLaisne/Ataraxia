using TMPro;
using UnityEngine;

namespace Exion.Handler
{
    public class StatusHandlerUI : MonoBehaviour
    {
        public StatusHandler status;
        public TextMeshProUGUI statusName;

        private void Start()
        {
            statusName.text = status.status.name;
            if (status.stacks > 1) statusName.text += " x" + status.stacks;
        }
    }
}