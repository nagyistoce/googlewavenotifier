using System;

namespace GoogleWaveNotifier
{
    public class WaveEventArgs : EventArgs
    {
        public Wave Wave { get; set; }

        public WaveEventArgs(Wave wave)
        {
            Wave = wave;
        }
    }
}