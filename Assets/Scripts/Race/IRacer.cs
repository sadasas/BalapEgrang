using Player;

namespace Race
{
    public interface IRacer
    {
        public string ID { get; set; }
        public void WaitStart(Pos currentPos);
        public void StartRace();
        public void FinishRace();
    }

}
