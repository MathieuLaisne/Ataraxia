using Exion.Ataraxia.ScriptableObjects;
using System;

namespace Exion.Ataraxia.Default
{
    [Serializable]
    public class Modifier
    {
        public ModifierType Name;
        public Status statusType;
        public float Amount;
    }


    public enum ModifierType
    {
        DEAL_MENTAL_DAMAGE,
        DEAL_DIRECT_MENTAL_DAMAGE,
        REMOVE_ALL_MENTAL,

        DESTROY_MENTAL_BARRIERS,

        DEAL_INSANITY_DAMAGE,
        DEAL_INSANITY_PER_MAX_MENTAL,
        DEAL_INSANITY_PER_BLOCKED,

        DEAL_HEALTH_DAMAGE,
        RANDOM_MAX_HEALTH_CHANGE,

        SUSPICION_PER_BLOCKED,
        SUSPICION_PER_UNBLOCKED,
        SUSPICION_FLAT,
        SUSPICION_PER_FRIEND,
        SUSPICION_PER_AFFECTED,
        SUSPICION_PER_FRIEND_CHANCE,

        APPLY_EFFECT_TO_BUILDING,
        APPLY_EFFECT_TO_CHARACTER,
        APPLY_EFFECT_TO_ALL_IN_BUILDING,

        SUMMON_MONSTER,
        
        CREATE_RANDOM_DRUG,
        
        TICK_BOMB,

        RUIN_BUILDING,

        REPEAT_IF_UNBLOCKED,

        DO_NOT_DESTROY,
        DO_NOT_DESTROY_IF_KILLS,

        EMPTY_HAND
    }

}
