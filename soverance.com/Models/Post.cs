using System;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public string Slug { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public Category Category { get; set; }
    }
}
