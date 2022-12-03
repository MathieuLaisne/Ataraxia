using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Exion.Ataraxia.Handler
{
    public class StatusHandlerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public StatusHandler status;
        public TextMeshProUGUI statusName;

        public void Init()
        {
            statusName.text = status.status.name;
            if (status.stacks > 1) statusName.text += " x" + status.stacks;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.m_instance.SetTooltip(status.status.description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.m_instance.HideTooltip();
        }

    }
}