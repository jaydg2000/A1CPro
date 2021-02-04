using System;

namespace A1CPro.Domain
{
    [Serializable]
    public class DiaryEntry
    {
        public int Id { get; set; }
        public DateTime DateOfReading { get; set; }
        public int Sugar { get; set; }
        public double Weight { get; set; }
        public BloodPressureReading BloodPressure { get; set; }
        public bool MorningMedsTaken { get; set; }
        public bool EveningMedsTaken { get; set; }

        public Boolean IsNew()
        {
            return Id == -1;
        }

        public void Patch(DiaryEntry entry)
        {
            DateOfReading = entry.DateOfReading;
            Sugar = entry.Sugar;
            Weight = entry.Weight;
            MorningMedsTaken = entry.MorningMedsTaken;
            EveningMedsTaken = entry.EveningMedsTaken;
            BloodPressure = new BloodPressureReading
            {
                SystolicBloodPressue = entry.BloodPressure.SystolicBloodPressue,
                DiastolicBloodPressure = entry.BloodPressure.DiastolicBloodPressure
            };
        }
    }
}
