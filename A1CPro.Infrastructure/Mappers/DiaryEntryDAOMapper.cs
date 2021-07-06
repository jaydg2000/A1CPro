using A1CPro.Data.Models;
using A1CPro.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1CPro.Data.Mappers
{
    public class DiaryEntryDAOMapper
    {
        public DiaryEntryDAO Map(DiaryEntry source)
        {
            return new DiaryEntryDAO
            {
                ID = source.Id,
                DateOfReading = source.DateOfReading,
                Sugar = source.Sugar,
                Weight = source.Weight,
                EveningMedsTaken = source.EveningMedsTaken,
                MorningMedsTaken = source.MorningMedsTaken,
                BloodPressureSystolic = source.BloodPressure == null ? 0 : source.BloodPressure.SystolicBloodPressue,
                BloodPressureDiastolic = source.BloodPressure == null ? 0 : source.BloodPressure.DiastolicBloodPressure
            };
        }
    }
}
