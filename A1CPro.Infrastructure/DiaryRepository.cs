using A1CPro.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A1CPro.Data
{
    public class DiaryRepository
    {
        private List<DiaryEntry> _diary;
        private DateTime _cachedDiaryStartDate;
        private DateTime _cachedDiaryEndDate;

        public IReadOnlyList<DiaryEntry> GetDiary(DateTime startDate, DateTime endDate)
        {
            if (_diary == null || !startDate.Date.Equals(_cachedDiaryStartDate.Date) || !endDate.Date.Equals(_cachedDiaryEndDate.Date))
            {
                _diary = LoadMockDiary(startDate, endDate);
            }

            return _diary.AsReadOnly();
        }

        private List<DiaryEntry> LoadMockDiary(DateTime startDate, DateTime endDate)
        {
            var theDate = DateTime.Now;
            var numberOfDays = startDate.Subtract(endDate).Days;
            var random = new Random();
            var diary = new List<DiaryEntry>();
            for (int x = numberOfDays; x >= 0; x--)
            {
                diary.Add(new DiaryEntry
                {
                    Id = x,
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

            return diary;
        }

        public int SaveDiaryEntry(DiaryEntry entry)
        {
            if (entry.Id == -1)
            {
                entry.Id = _diary.Max(e => e.Id) + 1;
                _diary.Add(entry);
            }
            else
            {
                var existingEntry = _diary.FirstOrDefault(e => e.Id == entry.Id);
                existingEntry?.Patch(entry);
            }

            return entry.Id;
        }
    }
}
