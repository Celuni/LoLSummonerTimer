using System;

namespace Loltimer.ChampionTimer
{
    public class ChampionTimerModel
    {
        private int id;
        private String championName;
        private TimerWithSynthesizer timer;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public String ChampionName
        {
            get { return championName; }
            set { championName = value; }
        }

        public ChampionTimerModel(int id, string championName)
        {
            this.id = id;
            this.championName = championName;
            timer = new TimerWithSynthesizer();
        }

        public int GetGoalTime(){
            return this.timer.GoalTime;
        }
        public TimerWithSynthesizer GetTimer()
        {
            return this.timer;
        }
    }
}
