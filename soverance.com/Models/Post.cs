using System;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public int PostType { get; set; }
        public string Slug { get; set; }
        public string Date { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        
        public string VideoUrl { get; set; }
        public string Slider1 { get; set; }
        public string Slider2 { get; set; }
        public string Slider3 { get; set; }

        public Category Category { get; set; }
    }
}
