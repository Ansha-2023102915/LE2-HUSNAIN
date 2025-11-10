using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private readonly string _connectionStringName = "Default";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        // Users
        public Task<int> CreateUser(UserModels user)
        {
            var sql = "INSERT INTO dbo.Users (Username, Password, FirstName, LastName) VALUES (@Username, @Password, @FirstName, @LastName)";
            return _db.SaveData(sql, user, _connectionStringName);
        }

        public async Task<UserModels> GetUserByUsernameAndPassword(string username, string password)
        {
            var sql = "SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password";
            var users = await _db.LoadData<UserModels, dynamic>(sql, new { Username = username, Password = password }, _connectionStringName);
            return users.FirstOrDefault();
        }

        // Posts
        public Task<int> CreatePost(PostModels post)
        {
            var sql = "INSERT INTO dbo.Posts (Title, Content, UserId) VALUES (@Title, @Content, @UserId)";
            return _db.SaveData(sql, post, _connectionStringName);
        }

        public Task<IEnumerable<ListPostModels>> GetAllPosts()
        {
            var sql = @"SELECT p.Id, p.Title, u.Username AS AuthorName, p.CreatedAt
                        FROM dbo.Posts p
                        JOIN dbo.Users u ON p.UserId = u.Id
                        ORDER BY p.CreatedAt DESC";
            return _db.LoadData<ListPostModels, dynamic>(sql, new { }, _connectionStringName);
        }

        public async Task<PostModels> GetPostById(int id)
        {
            var sql = @"SELECT p.Id, p.Title, p.Content, p.UserId, p.CreatedAt FROM dbo.Posts p WHERE p.Id = @Id";
            var posts = await _db.LoadData<PostModels, dynamic>(sql, new { Id = id }, _connectionStringName);
            return posts.FirstOrDefault();
        }
    }
}
