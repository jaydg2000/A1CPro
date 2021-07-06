using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace A1CPro.Data.Models
{
    public class DiaryEntryDAO
    {
        public static readonly int ID_NEW = -1;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime DateOfReading { get; set; }
        public int Sugar { get; set; }
        public double Weight { get; set; }
        public int BloodPressureSystolic { get; set; }
        public int BloodPressureDiastolic { get; set; }
        public bool MorningMedsTaken { get; set; }
        public bool EveningMedsTaken { get; set; }

    }
}
