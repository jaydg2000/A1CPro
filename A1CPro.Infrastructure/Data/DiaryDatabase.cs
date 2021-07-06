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
            _database = new SQLiteAsyncConnection(dbPath, true);
            _database.CreateTableAsync<DiaryEntryDAO>().Wait();
        }

        public async Task<List<DiaryEntryDAO>> GetDiary(DateTime startDate, DateTime endDate)
        {
            var rows = await _database.Table<DiaryEntryDAO>()
                //.Where(e => e.DateOfReading >= startDate.Date && e.DateOfReading <= endDate.Date)
                .OrderByDescending(t => t.DateOfReading)
                .ToListAsync();
            return rows;
        }

        public async Task<int> SaveDiaryEntry(DiaryEntryDAO dao)
        {
            if (dao.ID == DiaryEntryDAO.ID_NEW)
            {
                return await _database.InsertAsync(dao);
            }
            else
            {
                return await _database.UpdateAsync(dao);
            }
        }

        public Task DeleteAll()
        {
            return _database.DeleteAllAsync<DiaryEntryDAO>();
        }
    }
}
