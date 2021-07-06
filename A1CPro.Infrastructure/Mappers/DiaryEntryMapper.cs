using A1CPro.Data.Models;
using A1CPro.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1CPro.Data.Mappers
{
    public class DiaryEntryMapper
    {
        public DiaryEntry Map(DiaryEntryDAO source)
        {
            return new DiaryEntry
            {
                Id = source.ID,
                Sugar = source.Sugar,
                DateOfReading = source.DateOfReading,
                Weight = source.Weight,
                EveningMedsTaken = source.EveningMedsTaken,
                MorningMedsTaken = source.MorningMedsTaken,
                BloodPressure = new BloodPressureReading
                {
                    SystolicBloodPressue = source.BloodPressureSystolic,
                    DiastolicBloodPressure = source.BloodPressureDiastolic
                }
            };
        }
    }
}
