using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public enum RaceState
    {
        STARTING,
        PLAYING,
        FINISHED,
    }

    public struct PlayerDataRace
    {

        public float Time;
        public int Rank;
        public int Respawned;
        public PlayerDataRace(float time, int rank) : this()
        {
            Time = time;
            Rank = rank;

        }

        public override string ToString()
        {
            return $"{Time} {Rank} {Respawned}";
        }


    }
    public class RaceManager : MonoBehaviour
    {
        public static RaceManager s_Instance;
        public const int maxRacers = 3;

        static float s_Timer = 0;
        static int s_racerfinisheds = 0;
        static RaceState m_state = RaceState.STARTING;

        Dictionary<GameObject, PlayerDataRace> m_racers;
        Transform[] m_startPos;


        private void Awake()
        {
            if (s_Instance != null) Destroy(s_Instance);
            s_Instance = this;
        }
        private void Start()
        {
            SetupRace();
        }

        private void Update()
        {
            if (m_state == RaceState.PLAYING)
            {
                s_Timer += Time.deltaTime;
                foreach (var racer in m_racers)
                {
                    var dis = TrackRacer(racer.Key);

                }
            }
        }


        public void RacerCrashed(GameObject racer)
        {
            if (m_state != RaceState.PLAYING) return;
            var data = m_racers[racer];
            data.Respawned++;

            m_racers[racer] = data;


        }
        public void RacerFinished(GameObject racerFinished)
        {

            s_racerfinisheds++;
            racerFinished.GetComponent<IRacer>().FinishRace();
            var data = m_racers[racerFinished];
            data.Rank = s_racerfinisheds;
            data.Time = s_Timer;
            m_racers[racerFinished] = data;

            if (s_racerfinisheds == maxRacers) RaceFinished();

        }
        public void RegisterRacer(GameObject newRacer)
        {


            var id = m_racers.Count;
            var heigth = newRacer.GetComponent<Collider>().bounds.extents.y;
            newRacer.transform.position = new Vector3(m_startPos[id].position.x, heigth, m_startPos[id].position.z);
            var racer = newRacer.GetComponent<IRacer>();

            m_racers.Add(newRacer, default);

            var pos = id == 0 ? Pos.LEFT : (id == 1 ? Pos.CENTER : Pos.RIGHT);
            racer.WaitStart(pos);

            if (IsAllRacerReady()) StartRace();
        }

        void SetupRace()
        {
            m_racers = new();
            m_startPos = Array.ConvertAll(GameObject.FindGameObjectsWithTag("StartPos"), (p => p.transform));
        }

        float TrackRacer(GameObject racer)
        {
            var startPos = new Vector3(racer.transform.position.x, racer.transform.position.y, m_startPos[0].position.z);
            return Vector3.Distance(startPos, racer.transform.position);
        }
        void StartRace()
        {
            m_state = RaceState.PLAYING;
            foreach (var racer in m_racers)
            {

                racer.Key.GetComponent<IRacer>().StartRace();
            }
        }

        void RaceFinished()
        {
            m_state = RaceState.FINISHED;
            foreach (var racer in m_racers)
            {
                Debug.Log(racer.Value.ToString());
            }

        }
        bool IsAllRacerReady()
        {
            if (m_racers.Count == maxRacers) return true;
            return false;
        }
    }

}