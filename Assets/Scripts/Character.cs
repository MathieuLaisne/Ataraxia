using System.Collections.Generic;
using UnityEngine;

public class Character
{
    private string m_name;
    public string Name {
        get { return m_name; }
    }
    private Job m_job;
    public Job Job {
        get { return m_job; }
        set { m_job = value; }
    }
    private List<Character> m_friends;
    public List<Character> Friends {
        get { return m_friends; }
    }
    private int m_maxHP;
    public int maxHP {
        get { return m_maxHP; }
        set { m_maxHP = value; }
    }
    private int m_maxMP;
    public int maxMental {
        get { return m_maxMP;}
        set { m_maxMP = value;}
    }
    private int m_mentalBarrier;
    public int Barrier {
        get { return m_mentalBarrier;}
        set { m_mentalBarrier = value;}
    }
    private List<string> m_status;
    public List<string> Statuses {
        get { return m_status;}
    }
    private int m_hp;
    public int HP {
        get { return m_hp;}
        set { m_hp = value;}
    }
    private int m_mp;
    public int Mental {
        get { return m_mp;}
        set { m_mp = value;}
    }
    private int m_insanity;
    public int Insanity {
        get { return m_insanity; }
        set { m_insanity = value; }
    }

    private Sprite m_profile;
    public Sprite Profile
    {
        get { return m_profile; }
    }

    public Character(string name, Job job, Sprite img, int maxHp, int maxMental, int barrier) 
    {
        m_name = name; m_job = job; m_profile = img; m_maxHP = maxHp; m_maxMP = maxMental; m_mentalBarrier = barrier;
        m_hp = maxHP; m_mp = maxMental;
        m_status = new List<string>(); m_friends = new List<Character>();
    }

    public void AddFriend(Character newFriend) {
        m_friends.Add(newFriend);
    }

    public void ApplyStatus(string status) {
        m_status.Add(status);
    }
}
