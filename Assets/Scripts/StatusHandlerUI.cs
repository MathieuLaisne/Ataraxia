using Exion.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Exion.Handler
{
    public class StatusHandlerUI : MonoBehaviour
    {
        public StatusHandler status;
        public TextMeshProUGUI statusName;

        void Start()
        {
            statusName.text = status.status.name;
            if (status.stacks > 1) statusName.text += " x" + status.stacks;
        }

    }
}
