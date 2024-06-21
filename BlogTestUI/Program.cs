using BlogDataLibrary.Data;
using BlogDataLibrary.Database.BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using BlogDataLibrary.Database;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;



namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlData db = GetConnection();

            Authenticate(db);

            Register(db);

            AddPost(db);

            ListPosts(db);

            ShowPostDetails(db);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

        }
        static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();
            ISqlDataAccess dbAccess = new SqlDataAccess(config);
            SqlData db = new SqlData(dbAccess);

            return db;
        }
        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            UserModel user = db.Authenticate(username, password);

            return user;
        }

        public static void Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid credentials.");
            }
            else
            {
                Console.WriteLine($"Welcome, {user.UsernName}");
            }
        }

        public static void Register(SqlData db)
        {
            Console.Write("Enter new username: ");
            var username = Console.ReadLine();

            Console.Write("Enter new password: ");
            var password = Console.ReadLine();

            Console.Write("Enter new first name: ");
            var firstName = Console.ReadLine();

            Console.Write("Enter new last name: ");
            var lastName = Console.ReadLine();

            db.Register(username, firstName, lastName, password);
        }

        private static void AddPost(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Write body: ");
            string body = Console.ReadLine();

            PostModel post = new PostModel
            {
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };

            db.AddPost(post);
        }

        private static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"{post.Id}. Title: {post.Title} by {post.UserName} [{post.DateCreated.ToString("yyyy-MM-dd")}]");

                string postBodyPreview = post.Body.Length > 20 ? post.Body.Substring(0, 20) : post.Body;
                Console.WriteLine($"{postBodyPreview}...");
                Console.WriteLine();
            }
        }
        private static void ShowPostDetails(SqlData db)
        {
            Console.Write("Enter a post ID: ");
            int id = Int32.Parse(Console.ReadLine());

            ListPostModel post = db.ShowPostDetail(id);
            Console.WriteLine(post.Title);
            Console.WriteLine($"by {post.FirstName} {post.LastName} [{post.UserName}]");

            Console.WriteLine();

            Console.WriteLine(post.Body);

            Console.WriteLine(post.DateCreated.ToString("MMM d yyyy"));
        }
    }
}
