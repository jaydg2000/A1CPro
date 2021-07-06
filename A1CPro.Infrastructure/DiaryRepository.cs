using A1CPro.Data.Data;
using A1CPro.Data.Mappers;
using A1CPro.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A1CPro.Data
{
    public class DiaryRepository
    {
        private List<DiaryEntry> _diary;
        private DateTime _cachedDiaryStartDate;
        private DateTime _cachedDiaryEndDate;

        public async Task<IReadOnlyList<DiaryEntry>> GetDiary(DateTime startDate, DateTime endDate, bool forceReload = false)
        {
            await EraseAllEntries();
            if (forceReload 
                || _diary == null 
                || !startDate.Date.Equals(_cachedDiaryStartDate.Date) 
                || !endDate.Date.Equals(_cachedDiaryEndDate.Date))
            {
                _diary = await ReadDiaryFromStore(startDate, endDate);

                // ---------- TEMP ----------
                //if (_diary.Count < 1)
                //{
                //_diary = LoadMockDiary(startDate, endDate);
                //mockDiary.ForEach(async d => await SaveDiaryEntry(d));
                //_diary = await ReadDiaryFromStore(startDate, endDate);
                //}
            }

            return _diary.AsReadOnly();
        }

        private async Task<List<DiaryEntry>> ReadDiaryFromStore(DateTime startDate, DateTime endDate)
        {
            var mapper = new DiaryEntryMapper();
            var db = DiaryDatabase.Instance;
            var dbModels = await db.GetDiary(startDate, endDate);

            if (dbModels.Count > 0)
            {
                _cachedDiaryStartDate = startDate.Date;
                _cachedDiaryEndDate = endDate.Date;
                return dbModels.Select(m => mapper.Map(m)).ToList();
            }
            return new List<DiaryEntry>();
        }

        private List<DiaryEntry> LoadMockDiary(DateTime startDate, DateTime endDate)
        {
            var theDate = DateTime.Now;
            var numberOfDays = startDate.Subtract(endDate).Days;
            var random = new Random();
            var diary = new List<DiaryEntry>();
            for (int x = numberOfDays; x >= 0; x--)
            {
                // simulate missing some days
                if (random.Next(1, 10) < 3 )
                {
                    continue;
                }

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

        public async Task<int> SaveDiaryEntry(DiaryEntry entry)
        {
            var mapper = new DiaryEntryDAOMapper();
            var dao = mapper.Map(entry);
            return await DiaryDatabase.Instance.SaveDiaryEntry(dao);
        }

        public async Task EraseAllEntries()
        {
            await DiaryDatabase.Instance.DeleteAll();
        }
    }
}
