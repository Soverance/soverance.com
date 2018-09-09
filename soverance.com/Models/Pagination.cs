using System;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Pagination
    {
        public List<Post> Posts { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount { get; set; }

        public void GetPostsInPage()
        {

        }
    }
}
