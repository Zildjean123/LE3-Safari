﻿using BlogDataLibrary.Models;
using System.Collections.Generic;

namespace BlogDataLibrary.Data
{
    public interface ISqlData1
    {
        void AddPost(PostModel post);
        UserModel Authenticate(string username, string password);
        List<ListPostModel> ListPosts();
        void Register(string username, string firstname, string lastname, string password);
        ListPostModel ShowPostDetail(int id);
    }
}