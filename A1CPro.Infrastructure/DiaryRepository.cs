using A1CPro.Domain;
using System;
using System.Collections.Generic;

namespace A1CPro.Data
{
    public class DiaryRepository
    {
        public IReadOnlyList<DiaryEntry> GetDiary(DateTime startDate, DateTime endDate)
        {
            var theDate = DateTime.Now;
            var numberOfDays = startDate.Subtract(endDate).Days;
            var random = new Random();
            var diary = new List<DiaryEntry>();
            for (int x = numberOfDays; x >= 0; x--)
            {
                diary.Add(new DiaryEntry
                {
                    Id = Guid.NewGuid(),
                    DateOfReading = theDate,
                    Sugar = random.Next(1,10) > 1 ? random.Next(70, 140) : 0,
                    Weight = random.Next(1,10) > 2 ? random.Next(200,205) : 0,
                    MorningMedsTaken = random.Next(100) > 20,
                    EveningMedsTaken = random.Next(100) > 20,
                    BloodPressure = random.Next(1,10) > 7 ? 
                        new BloodPressureReading
                        {
                            SystolicBloodPressue = random.Next(110,130),
                            DiastolicBloodPressure = random.Next(70,85)
                        }
                        : null
                });
                theDate = theDate.AddDays(-1);
            }

            return diary.AsReadOnly();
        }
    }
}
