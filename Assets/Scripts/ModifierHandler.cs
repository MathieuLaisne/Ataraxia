using Exion.Ataraxia.Default;
using Exion.Ataraxia.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Ataraxia.Handler
{
    public class ModifierHandler
    {
        public static bool ApplyModifier(Modifier[] Modifiers, Modifier m, Character CH, BuildingHandler BH, ref float suspicion, ref int bombCounter, ref List<Card> jobDeck, Card RandomDrug, out bool emptyHand)
        {
            bool effectApplied = false;
            emptyHand = false;
            switch (m.Name)
            {
                case ModifierType.DEAL_MENTAL_DAMAGE:
                    if (CH != null)
                    {
                        effectApplied = true;
                        int blocked = CH.DealMentalDamage((int)m.Amount);
                        if (Array.Exists(Modifiers, m => m.Name == ModifierType.DEAL_INSANITY_PER_BLOCKED))
                        {
                            CH.MakeInsane(blocked);
                        }
                        Modifier modSusBlocked = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.SUSPICION_PER_BLOCKED);
                        if (modSusBlocked != null)
                        {
                            suspicion += (modSusBlocked.Amount * blocked);
                        }
                        Modifier modSusUnblocked = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.SUSPICION_PER_UNBLOCKED);
                        if (modSusBlocked != null)
                        {
                            suspicion += (modSusUnblocked.Amount * ((int)m.Amount - blocked));
                        }

                        Modifier modRepeatUnblocked = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.REPEAT_IF_UNBLOCKED);
                        if (modRepeatUnblocked != null && blocked == 0)
                        {
                            for (int repeat = 0; repeat < modRepeatUnblocked.Amount; repeat++)
                            {
                                CH.DealMentalDamage((int)m.Amount);
                            }
                        }

                        Modifier modDNDOnKill = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.DO_NOT_DESTROY_IF_KILLS);
                        if (modDNDOnKill != null && CH.corrupted)
                        {
                            effectApplied = false;
                        }
                    }
                    break;
                case ModifierType.DEAL_DIRECT_MENTAL_DAMAGE:
                    if (CH != null)
                    {
                        effectApplied = true;
                        CH.DealDirectMentalDamage((int)m.Amount);

                        Modifier modDNDOnKill = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.DO_NOT_DESTROY_IF_KILLS);
                        if (modDNDOnKill != null && CH.corrupted)
                        {
                            effectApplied = false;
                        }
                    }
                    break;
                case ModifierType.REMOVE_ALL_MENTAL:
                    if (CH != null)
                    {
                        effectApplied = true;
                        CH.DealMentalDamage(CH.Mental);
                    }
                    break;
                case ModifierType.DESTROY_MENTAL_BARRIERS:
                    if (CH != null)
                    {
                        effectApplied = true;
                        CH.DestroyMentalBarrier();
                    }
                    break;
                case ModifierType.DEAL_INSANITY_DAMAGE:
                    if (CH != null)
                    {
                        effectApplied = true;
                        suspicion += CH.MakeInsane((int)m.Amount);
                    }
                    break;
                case ModifierType.DEAL_INSANITY_PER_MAX_MENTAL:
                    if (CH != null)
                    {
                        effectApplied = true;
                        suspicion += CH.MakeInsane(CH.maxMental * (int)m.Amount);
                    }
                    break;
                case ModifierType.DEAL_INSANITY_PER_BLOCKED:
                    //Is done in a DEAL MENTAL DAMAGE Routine
                    break;
                case ModifierType.DEAL_HEALTH_DAMAGE:
                    if (CH != null)
                    {
                        effectApplied = true;
                        bool died = CH.DealHealthDamage((int)m.Amount);

                        Modifier modDNDOnKill = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.DO_NOT_DESTROY_IF_KILLS);
                        if (modDNDOnKill != null && died)
                        {
                            effectApplied = false;
                        }
                    }
                    break;
                case ModifierType.RANDOM_MAX_HEALTH_CHANGE:
                    if (CH != null)
                    {
                        effectApplied = true;
                        CH.Fleshwarp((int)m.Amount);
                    }
                    break;
                case ModifierType.SUSPICION_PER_BLOCKED:
                    //Is done in a DEAL MENTAL DAMAGE Routine
                    break;
                case ModifierType.SUSPICION_PER_UNBLOCKED:
                    //Is done in a DEAL MENTAL DAMAGE Routine
                    break;
                case ModifierType.SUSPICION_FLAT:
                    if (Modifiers.Length == 1 || effectApplied)
                    {
                        suspicion += m.Amount;
                    }
                    break;
                case ModifierType.SUSPICION_PER_FRIEND:
                    if (CH != null)
                    {
                        effectApplied = true;
                        Modifier modFriendChance = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.SUSPICION_PER_FRIEND_CHANCE);
                        if (modFriendChance != null)
                        {
                            foreach (Character f in CH.Friends)
                            {
                                if (UnityEngine.Random.Range(0, 100) >= modFriendChance.Amount)
                                {
                                    suspicion += m.Amount;
                                }
                            }
                            break;
                        }
                        suspicion += m.Amount * CH.Friends.Count;
                    }
                    break;
                case ModifierType.SUSPICION_PER_AFFECTED:
                    //Is done in APPLY EFFECT TO ALL IN BUILDING Routine
                    break;
                case ModifierType.SUSPICION_PER_FRIEND_CHANCE:
                    //Is done in SUSPICION PER FRIEND Routine
                    break;
                case ModifierType.APPLY_EFFECT_TO_BUILDING:
                    if (BH != null)
                    {
                        effectApplied = true;
                        int amount = m.statusType.name == "Trapped" ? bombCounter : 1;
                        BH.ApplyStatus(m.statusType, amount);
                    }
                    break;
                case ModifierType.APPLY_EFFECT_TO_CHARACTER:
                    if (CH != null)
                    {
                        effectApplied = true;
                        CH.ApplyStatus(m.statusType, 1);
                    }
                    break;
                case ModifierType.APPLY_EFFECT_TO_ALL_IN_BUILDING:
                    if (BH != null)
                    {
                        effectApplied = true;
                        foreach (Character resident in BH.building.Residents)
                        {
                            resident.ApplyStatus(m.statusType, 1);
                        }
                        foreach (Character worker in BH.building.Workers)
                        {
                            worker.ApplyStatus(m.statusType, 1);
                        }

                        int Affected = BH.building.Workers.Count + BH.building.Residents.Count;

                        Modifier modSusAffected = Array.Find<Modifier>(Modifiers, m => m.Name == ModifierType.SUSPICION_PER_AFFECTED);
                        if (modSusAffected != null)
                        {
                            suspicion += (modSusAffected.Amount * Affected);
                        }
                    }
                    break;
                case ModifierType.SUMMON_MONSTER:
                    Debug.Log("To implement summoning structure");
                    break;
                case ModifierType.CREATE_RANDOM_DRUG:
                    jobDeck.Add(RandomDrug);
                    break;
                case ModifierType.TICK_BOMB:
                    bombCounter++;
                    break;
                case ModifierType.RUIN_BUILDING:
                    if (BH != null)
                    {
                        if (BH.building.Type.name == "Abandonned Building") Debug.Log("wrong building");
                        else
                        {
                            effectApplied = true;
                            BH.building.Destroy();
                        }
                    }
                    break;
                case ModifierType.REPEAT_IF_UNBLOCKED:
                    //Is done in DEAL MENTAL DAMAGE Routine
                    break;
                case ModifierType.DO_NOT_DESTROY: //Why does it exists?!
                    effectApplied = false;
                    break;
                case ModifierType.DO_NOT_DESTROY_IF_KILLS:
                    //Is done in all DEAL MENTAL DAMAGE Routines and DEAL HEALTH DAMAGE Routine
                    break;
                case ModifierType.EMPTY_HAND:
                    emptyHand = true;
                    break;
                default:
                    Debug.Log("Not Implemented yet");
                    break;
            }
            return effectApplied;
        }

        public static void ApplyModifierInBuilding(Modifier m, Character CH, BuildingHandler BH)
        {
            Modifier[] Modifiers = { }; float sus = 0; int bomb = 0; List<Card> list = new List<Card>(); Card rnd = null; bool empty; //unused
            ApplyModifier(Modifiers, m, CH, BH, ref sus, ref bomb, ref list, rnd, out empty);
        }

        public static void AppluModifierOnCharacter(Modifier m, Character CH)
        {
            Modifier[] Modifiers = { }; float sus = 0; int bomb = 0; List<Card> list = new List<Card>(); Card rnd = null; bool empty; BuildingHandler BH = null; //unused
            ApplyModifier(Modifiers, m, CH, BH, ref sus, ref bomb, ref list, rnd, out empty);
        }
    }
}
