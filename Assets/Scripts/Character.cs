using Exion.Ataraxia.Handler;
using Exion.Ataraxia.ScriptableObjects;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Exion.Ataraxia.Default
{
    [System.Serializable]
    public class Character
    {
        private string m_name;

        public bool IsUntargettable = false;

        public string Name
        {
            get { return m_name; }
        }

        [SerializeField]
        private Job m_job;

        public Job Job
        {
            get { return m_job; }
            set { m_job = value; }
        }

        private List<Character> m_friends;

        public List<Character> Friends
        {
            get { return m_friends; }
        }

        private int m_maxHP;

        public int maxHP
        {
            get { return m_maxHP; }
            set { m_maxHP = value; }
        }

        private int m_maxMP;

        public int maxMental
        {
            get { return m_maxMP; }
            set { m_maxMP = value; }
        }

        private int m_mentalBarrier;

        public int Barrier
        {
            get { return m_mentalBarrier; }
            set { m_mentalBarrier = value; }
        }

        private List<StatusHandler> m_status;

        public List<StatusHandler> Statuses
        {
            get { return m_status; }
        }

        private int m_hp;

        public int HP
        {
            get { return m_hp; }
            set { m_hp = value; }
        }

        private int m_mp;

        public int Mental
        {
            get { return m_mp; }
            set { m_mp = value; }
        }

        private int m_insanity;

        public int Insanity
        {
            get { return m_insanity; }
            set { m_insanity = value; }
        }

        private Sprite m_profile;

        public Sprite Profile
        {
            get { return m_profile; }
        }

        private int str = 1;
        public int Strength
        {
            get
            {
                int internal_str = str;
                foreach (StatusHandler SH in m_status)
                {
                    if (SH.status.name == "Strength") internal_str += 2 * SH.stacks;
                }
                return internal_str;
            }
        }

        public bool corrupted = false;

        public Character(string name, Job job, Sprite img, int maxHp, int maxMental, int barrier)
        {
            m_name = name; m_job = job; m_profile = img; m_maxHP = maxHp; m_maxMP = maxMental; m_mentalBarrier = barrier;
            m_hp = maxHP; m_mp = maxMental;
            m_status = new List<StatusHandler>(); m_friends = new List<Character>();
            str = Random.Range(1, 5);
        }

        public void AddFriend(Character newFriend)
        {
            m_friends.Add(newFriend);
        }

        public void ApplyStatus(Status status, int stack)
        {
            bool toAdd = true;
            foreach (StatusHandler SH in m_status)
            {
                if (SH.status == status)
                {
                    SH.stacks += stack;
                    toAdd = false;
                }
            }
            if(status.name == "Untargettable")
            {
                IsUntargettable = true;
            }
            if (toAdd) m_status.Add(new StatusHandler(status, stack));
        }

        public int DealMentalDamage(int damage)
        {
            int blocked = 0;
            for (int i = damage; i > 0; i--)
            {
                if (m_mentalBarrier > 0)
                {
                    m_mentalBarrier--;
                    blocked++;
                }
                else m_mp--;
                if (m_mp == 0) break;
            }
            if (m_mp <= 0) corrupted = true;
            return blocked;
        }

        public bool DealDirectMentalDamage(int damage)
        {
            m_mp = Mathf.Clamp(m_mp - damage, 0, m_maxMP);
            if (m_mp == 0)
            {
                corrupted = true;
                return true;
            }
            return false;
        }

        public bool DealHealthDamage(int damage)
        {
            m_hp = Mathf.Clamp(m_hp - damage, 0, m_maxHP);
            if (m_hp == 0) return true;
            return false;
        }

        public float DestroyMentalBarrier()
        {
            m_mentalBarrier = 0;
            if (Random.Range(0, 100) < 50) return 0.5f * m_friends.Count;
            return 0;
        }

        public float MakeInsane(int damage)
        {
            m_insanity += damage;
            if (m_insanity >= 100)
            {
                corrupted = true;
                m_insanity = 100;
                return m_friends.Count * 2f; //suspicion generated per friends if person went crazy.
            }
            return 0;
        }

        public bool HasStatus(string statusName)
        {
            foreach (StatusHandler status in Statuses)
            {
                if (status.status.name == statusName) return true;
            }
            return false;
        }

        public void Fleshwarp(int percentage)
        {
            int sign = Random.Range(0, 1) * 2 - 1;
            m_maxHP += sign * percentage * m_maxHP;
            m_hp += sign * percentage * m_hp;
        }
    }
}