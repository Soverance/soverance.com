using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public Category Category { get; set; }

        //public SelectList CategoryDropDownList { get; set; }
    }
}
