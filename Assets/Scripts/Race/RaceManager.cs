using Player;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using System.Linq;

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
        public bool IsPlayer;
        public float Distance;
        public GameObject GameObject;


        public PlayerDataRace(float time, int rank, bool isPlayer = false) : this()
        {
            Time = time;
            Rank = rank;
            IsPlayer = isPlayer;
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

        float m_timer = 0;
        int m_racerFinisheds = 0;

        Dictionary<string, PlayerDataRace> m_racers;

        [SerializeField] Transform[] m_startPos;

        public RaceState s_State { get; private set; } = RaceState.STARTING;



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
            if (s_State == RaceState.PLAYING)
            {
                m_timer += Time.deltaTime;
                foreach (var key in m_racers.Keys.ToArray())
                {
                    TrackRacer(key);
                }

                CalculateRankPlayer();

            }
        }


        public void RacerCrashed(IRacer racer)
        {
            if (s_State != RaceState.PLAYING) return;
            var data = m_racers[racer.ID];
            data.Respawned++;

            m_racers[racer.ID] = data;


        }
        public void RacerFinished(IRacer racerFinished)
        {

            m_racerFinisheds++;
            racerFinished.FinishRace();
            var data = m_racers[racerFinished.ID];
            data.Rank = m_racerFinisheds;
            data.Time = m_timer;
            m_racers[racerFinished.ID] = data;
            if (racerFinished.ID == "PLAYER")
            {
                var dataPlayer = m_racers[racerFinished.ID];
                StageManager.s_Instance.CheckForNewRecord(dataPlayer.Time, dataPlayer.Rank, dataPlayer.Respawned);
                StartCoroutine(FinishingRace());
            }
            if (m_racerFinisheds == maxRacers) RaceFinished();

        }
        public void RegisterRacer(string guid, GameObject newRacer, bool isPlayer)
        {


            var id = m_racers.Count;
            var heigth = newRacer.GetComponent<Collider>().bounds.min.y;
            newRacer.transform.position = new Vector3(m_startPos[id].position.x, heigth, m_startPos[id].position.z);
            var racer = newRacer.GetComponent<IRacer>();

            PlayerDataRace newData = default;
            newData.IsPlayer = isPlayer;
            newData.GameObject = newRacer;
            m_racers.Add(guid, newData);


            var pos = id == 0 ? Pos.LEFT : (id == 1 ? Pos.CENTER : Pos.RIGHT);
            racer.WaitStart(pos);

            if (IsAllRacerReady()) StartRace();
        }

        void CalculateRankPlayer()
        {
            var rank = 1;
            var disPlayer = m_racers["PLAYER"].Distance;
            foreach (var otherDis in m_racers.Values)
            {
                if (!otherDis.IsPlayer)
                {
                    if (otherDis.Distance > disPlayer) rank++;
                    else if (rank > 1) rank--;
                }

            }
            var rankHandler = UIManager.s_this.GetHUD(HUDType.RANK_RACER).GetComponent<RankRacerHandlerUI>();
            rankHandler.gameObject.SetActive(true);
            rankHandler.UpdateRank(rank);
        }

        void TrackRacer(string key)
        {
            var racerGameObject = m_racers[key].GameObject;

            var startPos = new Vector3(racerGameObject.transform.position.x, racerGameObject.transform.position.y, m_startPos[0].position.z);
            var dis = Vector3.Distance(startPos, racerGameObject.transform.position);
            var oldData = m_racers[key];
            oldData.Distance = dis;
            m_racers[key] = oldData;
        }

        void SetupRace()
        {
            m_racers = new();
        }

        void StartRace()
        {
            StartCoroutine(StartingRace());
        }

        void RaceFinished()
        {

            s_State = RaceState.FINISHED;
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


        IEnumerator StartingRace()
        {
            UIManager.s_this.ForceHUD(HUDType.COUNTDOWN_START);
            var countDownHandler = UIManager.s_this.GetHUD(HUDType.COUNTDOWN_START).GetComponent<CountDownStartHandlerUI>();
            var countDown = 3;
            while (countDown > 0)
            {
                countDownHandler.UpdateCountDown(countDown);
                countDown--;
                yield return new WaitForSeconds(2f);
            }
            UIManager.s_this.DisableHUD(HUDType.COUNTDOWN_START);

            s_State = RaceState.PLAYING;
            foreach (var racer in m_racers)
            {

                racer.Value.GameObject.GetComponent<IRacer>().StartRace();
            }
        }

        IEnumerator FinishingRace()
        {

            var statisticPlayerHUD = UIManager.s_this.GetHUD(HUDType.STATISTIC_PLAYER_RACE_FINISHED).GetComponent<StatisticPlayerHandlerUI>();
            statisticPlayerHUD.gameObject.SetActive(true);
            var dataRacePlayer = m_racers["PLAYER"];

            var rating = StageManager.s_Instance.CalculateRating(dataRacePlayer.Time);
            statisticPlayerHUD.UpdateText(rating, dataRacePlayer.Rank, dataRacePlayer.Time, dataRacePlayer.Respawned
            );




            yield return null;

        }
    }

}
