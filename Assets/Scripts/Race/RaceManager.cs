using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{

    public interface IRacer
    {
        public void WaitStart(Pos currentPos);
        public void StartRace();
        public void FinishRace();
    }
    public enum RaceState
    {
        STARTING,
        PLAYING,
        FINISHED,
    }
    public class RaceManager : MonoBehaviour
    {
        public static RaceManager s_Instance;
        public const int maxRacers = 1;

        RaceState m_state = RaceState.STARTING;
        List<GameObject> m_racers;
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
                foreach (var racer in m_racers)
                {
                    var dis = TrackRacer(racer);

                }
            }
        }


        public void RacerFinished(GameObject racerFinished)
        {
            foreach (var racer in m_racers)
            {
                if (racer == racerFinished)
                {
                    racer.GetComponent<IRacer>().FinishRace();
                }
            }
        }
        public void RegisterRacer(GameObject newRacer)
        {


            var id = m_racers.Count;
            var heigth = newRacer.GetComponent<Collider>().bounds.extents.y;
            newRacer.transform.position = new Vector3(m_startPos[id].position.x, heigth, m_startPos[id].position.z);
            var racer = newRacer.GetComponent<IRacer>();

            m_racers.Add(newRacer);

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

                racer.GetComponent<IRacer>().StartRace();
            }
        }
        bool IsAllRacerReady()
        {
            if (m_racers.Count == maxRacers) return true;
            return false;
        }
    }

}