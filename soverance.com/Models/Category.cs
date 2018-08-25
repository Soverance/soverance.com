using System;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Category
    {
        public Category()
        {
            Post = new HashSet<Post>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ICollection<Post> Post { get; set; }
    }
}
