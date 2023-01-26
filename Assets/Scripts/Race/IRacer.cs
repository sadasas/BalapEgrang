using Player;

namespace Race
{
    public interface IRacer
    {
        public void WaitStart(Pos currentPos);
        public void StartRace();
        public void FinishRace();
    }

}