using Exion.ScriptableObjects;
using UnityEngine;

namespace Exion.Handler
{

    public class StatusHandler : MonoBehaviour
    {
        public int stacks;
        public Status status;

        public StatusHandler(Status st, int stack)
        {
            stacks = stack; status = st;
        }
    }
}
