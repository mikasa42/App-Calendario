using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Calendar.Models;
using System.Formats.Tar;

namespace Calendar.Services
{
    public class MonthsDatabase
    {
        readonly SQLiteAsyncConnection _database;
        public MonthsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MonthData>().Wait();
        }

        public async Task<MonthData> GetMonthAsync(uint month, uint year)
        {
            var data =  await _database.Table<MonthData>()
                            .Where(d => d.Month == month && d.Year == year)
                            .FirstOrDefaultAsync();
            if (data == null) return await CreateMonthAsync(month, year);

            return data;
        }

        public Task<int> SaveMonthAsync(MonthData month) => month.Id != 0 ? _database.UpdateAsync(month) : throw new InvalidOperationException("Inexistent Month in Database, Month must be created before day state can be changed");

        public async Task SetDayStateAsync(uint day, uint month, uint year, bool isSelected)
        {
            var entry = await _database.Table<MonthData>()
                .Where(e => e.Month == month && e.Year == year)
                .FirstOrDefaultAsync();

            if (entry == null)
            {
                throw new InvalidOperationException("Inexistent Month in Database, Month must be created before day state can be changed");
            }

            if (day < 1 || day > 31)
                throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 31");

            uint bit = 1u << ((int)day - 1);

            if (isSelected)
                entry.DayStatesPacked |= bit; // Set bit
            else
                entry.DayStatesPacked &= ~bit; // Clear bit

            if (entry.Id == 0)
                await _database.InsertAsync(entry);
            else
                await _database.UpdateAsync(entry);
        }

        private async Task<MonthData> CreateMonthAsync(uint month, uint year)
        {
            var entry = new MonthData
            {
                Month = month,
                Year = year,
                DayStatesPacked = 0
            };

            await _database.InsertAsync(entry);

            return entry;
        }
    }
}

