namespace A1CPro.Domain
{
    public class BloodPressureReading
    {
        public int SystolicBloodPressue { get; set; }
        public int DiastolicBloodPressure { get; set; }

        public override string ToString()
        {
            return $"{SystolicBloodPressue}/{DiastolicBloodPressure}";
        }
    }
}