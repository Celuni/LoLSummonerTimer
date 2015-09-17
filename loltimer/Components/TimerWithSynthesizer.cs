using System;
using System.ComponentModel;
using System.Speech.Synthesis; //for speech synth
using System.Windows.Threading; //for dispatcher timer

namespace Loltimer
{
    public partial class TimerWithSynthesizer : Component
    {
        private readonly int flashTime = 300;
        private int goalTime;
        private int utilityPoints;
        private int time;
        private SpeechSynthesizer synth;
        private DispatcherTimer stopwatch;

        public int Time
        {
            get { return time; }
        }
        public int GoalTime
        {
            get { return goalTime; }
        }
        public int UtilityPoints
        {
            get { return utilityPoints; }
            set { utilityPoints = value; }
        }


        public TimerWithSynthesizer()
        {
            InitializeComponent();
            
            stopwatch = new DispatcherTimer();
            stopwatch.Interval = new TimeSpan(0, 0, 1); //every 1 second, a Tick Happens.
            stopwatch.Tick += TickAction;//action to take for every Tick

            //initialize Synth
            this.synth = new SpeechSynthesizer();
            InitializeSynthesizer();
        }

        public TimerWithSynthesizer(IContainer container)
        {
            container.Add(this);
            InitializeComponent();

            stopwatch = new DispatcherTimer();
            //initialize time = goal time and decrement.
            this.time = goalTime;
            this.stopwatch.Interval = new TimeSpan(0, 0, 1); //every 1 second, a Tick Happens.

            //initialize Synth
            this.synth = new SpeechSynthesizer();
            InitializeSynthesizer();
        }

        private void InitializeSynthesizer()
        {
            synth.SetOutputToDefaultAudioDevice();
            //synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult); // Unavailable until more installed voices are downloaded.
        }

        private void TickAction(object sender, EventArgs e)
        {
            //every tick, decrement;
            time--;
        }

        public void SayThis(string sentence)
        {
            synth.SpeakAsync(sentence);
        }

        public void StartTimer()
        {
            EvauluateTimeFromUtility();
            this.time = goalTime;
            this.stopwatch.Start();
        }

        public void ResetTimer() //Does not start, just reset to default values
        {
            this.stopwatch.Stop();
            time = goalTime;
            
        }

        public void AddActionToTick(EventHandler e)
        {
            this.stopwatch.Tick += e;
        }

        public void EvauluateTimeFromUtility()
        {
            double percentageReduced = 0;
            switch (utilityPoints)
            {
                case 0: percentageReduced = 0; break;
                case 1: percentageReduced = .08; break;
                case 2: percentageReduced = .14; break;
                case 3: percentageReduced = .20; break;
                   

            }
            goalTime = (int) (flashTime - (flashTime * percentageReduced));
            return;
        }

    }
}
