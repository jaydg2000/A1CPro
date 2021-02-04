using A1CPro.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace A1CPro.Data.Data
{
    public class DiaryDatabase
    {

        private static DiaryDatabase _instance;
        public static DiaryDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DiaryDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "A1CPro.db3"));
                }

                return _instance;
            }
        }

        private readonly SQLiteAsyncConnection _database;

        private DiaryDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DiaryEntryDAO>().Wait();
        }

        public Task<List<DiaryEntryDAO>> GetDiary(DateTime startDate, DateTime endDate)
        {
            return _database.Table<DiaryEntryDAO>()
                .Where(e => e.DateOfReading.Date >= startDate.Date && e.DateOfReading.Date <= endDate.Date)
                .ToListAsync();
        }

        public Task<int> SaveDiaryEntry(DiaryEntryDAO dao)
        {
            if (dao.ID == 0)
            {
                return _database.InsertAsync(dao);
            }
            else
            {
                return _database.UpdateAsync(dao);
            }
        }

        public Task DeleteAll()
        {
            return _database.DeleteAllAsync<DiaryEntryDAO>();
        }
    }
}
