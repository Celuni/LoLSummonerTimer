using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Speech.Synthesis;

namespace Loltimer
{
    public class TimerWithSpeech : DispatcherTimer
    {
        readonly private int goalTime = 10;
        private int utilityPoints;
        private int time;
        private SpeechSynthesizer synth;
        private ProgBarWithText parentProgBar;
        private ChampionPanel championPanelCopy;

        public int UtilityPoints
        {
            get { return utilityPoints; }
            set { utilityPoints = value; }
        }

        public TimerWithSpeech(ChampionPanel parent, ProgBarWithText progBar)
        {
            this.championPanelCopy = parent;
            //initialize time = goal time and decrement.
            this.time = goalTime;

            this.Interval = new TimeSpan(0, 0, 1); //every 1 second, a Tick Happens.
            this.Tick += tickAction;//action to take for every Tick


            this.parentProgBar = progBar;
            parentProgBar.SetProgMax(goalTime);


            //initialize Synth
            this.synth = new SpeechSynthesizer();
            InitializeSynthesizer();
        }

        private void InitializeSynthesizer()
        {
            synth.SetOutputToDefaultAudioDevice();
            //synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult); // Unavailable until more installed voices are downloaded.
        }

        private void tickAction(object sender, EventArgs e)
        {
            //every tick, decrement;
            time--;
            if (time < 1)
            {
                this.Stop();
                SayThis(championPanelCopy.ChampionName + "'s Flash is back up!");
                time = goalTime;
                //update buttons 
                //parentProgBar.UpdateProgBar(time);
                //hit reset in parent , null because caller and event are unnecessary
                championPanelCopy.ResetClick(null, null);

            }
            else
            {
                //update progress bars
                parentProgBar.UpdateProgBar(time);
            }

        }

        public void SayThis(string sentence)
        {
            synth.SpeakAsync(sentence);
        }

        public void ResetProgressBarAndTimer()
        {
            parentProgBar.ResetProgressBar();
            this.time = goalTime;
            this.Interval = new TimeSpan(0,0,1);

        }
    }
}
