using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exion.Handler
{
    public class TimeManager : MonoBehaviour
    {
        private int m_timeElapsed = 0;
        public string Time
        {
            get { return m_time; }
        }
        [SerializeField]
        private string m_time = "Morning";

        public bool pause = true;

        // Start is called before the first frame update
        void Update()
        {
            if (Input.GetButtonDown("Jump")) pause = !pause;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!pause)
            {
                m_timeElapsed++;
                switch (m_time)
                {
                    case "Morning":
                        if (m_timeElapsed == 15)
                        {
                            m_timeElapsed = 0;
                            m_time = "Work";
                        }
                        break;
                    case "Work":
                        if (m_timeElapsed == 30)
                        {
                            m_timeElapsed = 0;
                            m_time = "End Work";
                        }
                        break;
                    case "End Work":
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = "Free Time";
                        }
                        break;
                    case "Free Time":
                        if (m_timeElapsed == 20)
                        {
                            m_timeElapsed = 0;
                            m_time = "End Free";
                        }
                        break;
                    case "End Free":
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = "Night";
                        }
                        break;
                    case "Night":
                        if (m_timeElapsed == 10)
                        {
                            m_timeElapsed = 0;
                            m_time = "End Night";
                        }
                        break;
                    case "End Night":
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = "Morning";
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
