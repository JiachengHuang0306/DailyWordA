using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DailyWordA.Library.Helpers;
using DailyWordA.Library.Models;
using SQLite;

namespace DailyWordA.Library.Services;

public class CourseStorage : ICourseStorage
    {
        public const string DbName = "coursesdb.sqlite3";
        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection Connection => _connection ??= new SQLiteAsyncConnection(DbPath);
        
        public static readonly string DbPath = PathHelper.GetLocalFilePath(DbName);

        public bool IsInitialized { get; private set; }

        public async Task InitializeAsync()
        {
            if (!IsInitialized)
            {
                await Connection.CreateTableAsync<CourseObject>();
                IsInitialized = true;
            }
        }

        public async Task InitializeAsyncForFirstTime(IEnumerable<CourseObject> initialData)
        {
            await InitializeAsync();
            await Connection.InsertAllAsync(initialData);
        }

        public async Task<CourseObject> GetCourseAsync(int id)
        {
            return await Connection.Table<CourseObject>().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IList<CourseObject>> GetCoursesAsync(Expression<Func<CourseObject, bool>> where, int skip, int take)
        {
            return await Connection.Table<CourseObject>().Where(where).Skip(skip).Take(take).ToListAsync();
        }

        public async Task SaveCourseAsync(CourseObject courseObject)
        {
            await Connection.InsertOrReplaceAsync(courseObject);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await GetCourseAsync(id);
            if (course != null)
            {
                await Connection.DeleteAsync(course);
            }
        }

        public async Task<IList<CourseObject>> GetCoursesByDateAsync(DateTime date)
        {
            return await Connection.Table<CourseObject>().Where(c => c.Date.Date == date.Date).ToListAsync();
        }

        public async Task<CourseObject> GetRandomCourseAsync()
        {
            var count = await Connection.Table<CourseObject>().CountAsync();
            if (count == 0) return null;

            var randomIndex = new Random().Next(count);
            return await Connection.Table<CourseObject>().Skip(randomIndex).FirstOrDefaultAsync();
        }

        public async Task CloseAsync()
        {
            await Connection.CloseAsync();
        }
    }