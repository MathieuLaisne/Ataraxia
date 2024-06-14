using UnityEngine;
using UnityEngine.UI;

namespace Exion.Ataraxia.Handler
{
    public class TimeManager : MonoBehaviour
    {
        private int m_timeElapsed = 0;

        [SerializeField]
        private Light sun;

        public Image pauseIndicator;
        public Sprite pauseSprite;
        public Sprite morningSprite;
        public Sprite workSprite;
        public Sprite leisureSprite;
        public Sprite nightSprite;

        [SerializeField]
        private Color[] lightTime;

        private enum LightColour
        {
            Night = 0,
            Day = 1,
            Noon = 2
        }

        public TimePeriod Time
        {
            get { return m_time; }
        }

        [SerializeField]
        private TimePeriod m_time = TimePeriod.Morning;

        public bool pause = true;

        // Start is called before the first frame update
        private void Update()
        {
            if (Input.GetButtonDown("Jump")) pause = !pause;
            if (!pause)
            {
                switch (m_time)
                {
                    case TimePeriod.Morning:
                        sun.intensity += 0.00000001f;
                        sun.color = Color.Lerp(sun.color, lightTime[(int)LightColour.Day], 0.00005f);
                        break;

                    case TimePeriod.Work:
                        sun.color = Color.Lerp(sun.color, lightTime[(int)LightColour.Day], 0.00005f);
                        sun.intensity = Mathf.Clamp(sun.intensity + 0.00005f, 0.5f, 1);
                        break;

                    case TimePeriod.Free_Time:
                        sun.color = Color.Lerp(sun.color, lightTime[(int)LightColour.Noon], 0.000005f);
                        sun.intensity = Mathf.Clamp(sun.intensity - 0.00005f, 0.5f, 1);
                        break;

                    case TimePeriod.Night:
                        sun.color = Color.Lerp(sun.color, lightTime[(int)LightColour.Night], 0.00005f);
                        sun.intensity = Mathf.Clamp(sun.intensity - 0.00005f, 0.5f, 1);
                        break;

                    default:
                        break;
                }
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!pause)
            {
                m_timeElapsed++;
                switch (m_time)
                {
                    case TimePeriod.Morning:
                        if (pauseIndicator.sprite != morningSprite) pauseIndicator.sprite = morningSprite;
                        sun.color = Color.Lerp(sun.color, Color.white, 0.1f);
                        if (m_timeElapsed == 15)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.End_Morning;
                        }
                        break;

                    case TimePeriod.End_Morning:
                        sun.color = Color.Lerp(sun.color, Color.yellow, 0.01f);
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.Work;
                        }
                        break;

                    case TimePeriod.Work:
                        if (pauseIndicator.sprite != workSprite) pauseIndicator.sprite = workSprite;
                        sun.color = Color.Lerp(sun.color, Color.yellow, 0.1f);
                        if (m_timeElapsed == 30)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.End_Work;
                        }
                        break;

                    case TimePeriod.End_Work:
                        sun.color = Color.Lerp(sun.color, Color.yellow, 0.1f);
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.Free_Time;
                        }
                        break;

                    case TimePeriod.Free_Time:
                        if (pauseIndicator.sprite != leisureSprite) pauseIndicator.sprite = leisureSprite;
                        sun.color = Color.Lerp(sun.color, Color.red, 0.1f);
                        if (m_timeElapsed == 20)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.End_Free_Time;
                        }
                        break;

                    case TimePeriod.End_Free_Time:
                        sun.color = Color.Lerp(sun.color, Color.magenta, 0.1f);
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.Night;
                        }
                        break;

                    case TimePeriod.Night:
                        if (pauseIndicator.sprite != nightSprite) pauseIndicator.sprite = nightSprite;
                        sun.color = Color.Lerp(sun.color, Color.blue, 0.1f);
                        if (m_timeElapsed == 10)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.End_Night;
                        }
                        break;

                    case TimePeriod.End_Night:
                        if (m_timeElapsed == 1)
                        {
                            m_timeElapsed = 0;
                            m_time = TimePeriod.Morning;
                        }
                        break;

                    default:
                        break;
                }
            }
            else if (pauseIndicator.sprite != pauseSprite) pauseIndicator.sprite = pauseSprite;
        }
    }

    public enum TimePeriod
    {
        Morning,
        End_Morning,

        Work,
        End_Work,

        Free_Time,
        End_Free_Time,

        Night,
        End_Night
    }
}