using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogDataLibrary.Data
{
    public class SqlData
    {
        private ISqlDataAccess _db;
        private const string connectionStringName = "Default";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task RegisterUser(string username, string password, string firstName, string lastName)
        {
            string sql = @"INSERT INTO dbo.Users (UserName, Password, FirstName, LastName) 
                          VALUES (@UserName, @Password, @FirstName, @LastName)";

            return Task.Run(() =>
                _db.SaveData(sql, new { UserName = username, Password = password, FirstName = firstName, LastName = lastName },
                            connectionStringName, false));
        }

        public async Task<int?> Authenticate(string username, string password)
        {
            string sql = "SELECT Id FROM dbo.Users WHERE UserName = @UserName AND Password = @Password";

            var result = await Task.Run(() =>
                _db.LoadData<int, dynamic>(sql, new { UserName = username, Password = password },
                                         connectionStringName, false));

            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }

        public Task AddPost(int userId, string title, string content)
        {
            string sql = @"INSERT INTO dbo.Posts (UserId, Title, Content, CreatedAt) 
                          VALUES (@UserId, @Title, @Content, GETUTCDATE())";

            return Task.Run(() =>
                _db.SaveData(sql, new { UserId = userId, Title = title, Content = content },
                            connectionStringName, false));
        }

        public async Task<List<ListPostModel>> ListPosts()
        {
            string sql = @"SELECT p.Id, p.Title, p.Content, p.CreatedAt as DateCreated, 
                                  u.UserName, u.FirstName, u.LastName
                           FROM dbo.Posts p
                           INNER JOIN dbo.Users u ON p.UserId = u.Id
                           ORDER BY p.CreatedAt DESC";

            return await Task.Run(() =>
                _db.LoadData<ListPostModel, dynamic>(sql, new { }, connectionStringName, false));
        }

        public async Task<PostModels> GetPost(int postId)
        {
            string sql = @"SELECT p.Id, p.UserId, p.Title, p.Content, p.CreatedAt as DateCreated,
                          u.UserName, u.FirstName, u.LastName
                   FROM dbo.Posts p
                   INNER JOIN dbo.Users u ON p.UserId = u.Id
                   WHERE p.Id = @PostId";

            var results = await Task.Run(() =>
                _db.LoadData<PostModels, dynamic>(sql, new { PostId = postId }, connectionStringName, false));

            return results.Count > 0 ? results[0] : null;
        }
    }
}