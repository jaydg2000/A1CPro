using System;

namespace A1CPro.Domain
{
    public class DiaryEntry
    {
        public Guid Id { get; set; }
        public DateTime DateOfReading { get; set; }
        public int Sugar { get; set; }
        public double Weight { get; set; }
        public BloodPressureReading BloodPressure { get; set; }
        public bool MorningMedsTaken { get; set; }
        public bool EveningMedsTaken { get; set; }
    }
}
