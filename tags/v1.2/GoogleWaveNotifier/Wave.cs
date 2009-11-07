using System;

namespace GoogleWaveNotifier
{
    public class Wave : IEquatable<Wave>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }

        public bool Equals(Wave other)
        {
            return Id == other.Id;
        }
    }
}