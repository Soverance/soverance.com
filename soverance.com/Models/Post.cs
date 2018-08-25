using System;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int BlogsId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }

        public Blogs Blogs { get; set; }
    }
}
