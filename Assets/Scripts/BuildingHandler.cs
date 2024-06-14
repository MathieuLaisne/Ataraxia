using Exion.Ataraxia.Default;
using Exion.Ataraxia.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace Exion.Ataraxia.Handler
{
    public class BuildingHandler : MonoBehaviour
    {
        public Building building;

        public PlayerManager PM;

        public TimeManager timeManager;

        //Effect Action verifiers
        List<Modifier> ModifierRequireMeetCharacter;
        List<Modifier> ModifierRequireTime;
        List<Status>   StatusRequireAmount;

        [SerializeField]
        private List<GameObject> resident = new List<GameObject>();

        public void Start()
        {
            ModifierRequireMeetCharacter = new List<Modifier>();
            ModifierRequireTime = new List<Modifier>();
            StatusRequireAmount = new List<Status>();
            PM = FindObjectOfType<PlayerManager>();
            timeManager = FindObjectOfType<TimeManager>();
        }

        public void FixedUpdate()
        {
            if (ModifierRequireTime.Count > 0)
            {
                foreach (Modifier m in ModifierRequireTime)
                {
                    if(m.timePeriod == timeManager.Time)
                    {
                        switch(m.target)
                        {
                            case Target.WORKERS:
                                foreach (Character c in building.Workers)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                break;
                            case Target.RESIDENTS:
                                foreach (Character c in building.Residents)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                break;
                            case Target.ALL:
                                foreach (Character c in building.Residents)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                foreach (Character c in building.Workers)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                break;
                            default:
                                ModifierHandler.ApplyModifierInBuilding(m, null, this);
                                break;
                        }
                    }
                }
            }
            if(timeManager.Time == TimePeriod.End_Night)
            {
                foreach(Status s in StatusRequireAmount)
                {
                    Modifier m = Array.Find<Modifier>(s.Modifiers, modif => modif.RequirementType.Contains(RequirementType.AMOUNT_OF_STATUS_IS));
                    if(building.AmountStatus(s.name) == m.AmountRequired)
                    {
                        switch (m.target)
                        {
                            case Target.WORKERS:
                                foreach (Character c in building.Workers)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                break;
                            case Target.RESIDENTS:
                                foreach (Character c in building.Residents)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                break;
                            case Target.ALL:
                                foreach (Character c in building.Residents)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                foreach (Character c in building.Workers)
                                {

                                    ModifierHandler.ApplyModifierInBuilding(m, c, this);
                                }
                                break;
                            default:
                                ModifierHandler.ApplyModifierInBuilding(m, null, this);
                                break;
                        }
                    }
                }
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (ModifierRequireMeetCharacter.Count > 0)
            {
                if (other.gameObject.GetComponent<CharacterHandler>() != null)
                {
                    foreach (Modifier m in ModifierRequireMeetCharacter)
                    {
                        if (m.RequirementType.Contains(RequirementType.TIME_IS))
                        {
                            if (timeManager.Time == m.timePeriod)
                            {
                                ModifierHandler.ApplyModifierInBuilding(m, other.gameObject.GetComponent<CharacterHandler>().character, this);
                            }
                        }
                        else
                        {
                            ModifierHandler.ApplyModifierInBuilding(m, other.gameObject.GetComponent<CharacterHandler>().character, this);
                        }
                    }
                }
            }
        }

        public void AddResident(GameObject c)
        {
            resident.Add(c);
        }

        public void ApplyStatus(Status s, int amount)
        {
            foreach (Modifier m in s.Modifiers)
            {
                if(m.RequirementType.Contains(RequirementType.ON_MEET_CHARACTER))
                    ModifierRequireMeetCharacter.Add(m);
                if(m.RequirementType.Contains(RequirementType.TIME_IS))
                    ModifierRequireTime.Add(m);
                if(m.RequirementType.Contains(RequirementType.AMOUNT_OF_STATUS_IS))
                    StatusRequireAmount.Add(s);
            }
            building.ApplyStatus(s, amount);
        }
    }
}