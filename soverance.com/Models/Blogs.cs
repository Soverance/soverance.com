using System;
using System.Collections.Generic;

namespace soverance.com.Models
{
    public partial class Blogs
    {
        public Blogs()
        {
            Post = new HashSet<Post>();
        }

        public int BlogsId { get; set; }
        public string Url { get; set; }

        public ICollection<Post> Post { get; set; }
    }
}
