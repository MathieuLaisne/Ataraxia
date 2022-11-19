using Exion.Ataraxia.ScriptableObjects;

namespace Exion.Ataraxia.Handler
{
    public class StatusHandler
    {
        public int stacks;
        public Status status;

        public StatusHandler(Status st, int stack)
        {
            stacks = stack; status = st;
        }
    }
}