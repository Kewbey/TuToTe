using System;
using System.Collections.Generic;
using System.Text;

namespace TuToTe.DataTypes.Helper
{
    public struct PostToProcess
    {
        public string PostLink, BlogTitle, BlogId;

        public PostToProcess(string postLink, string blogTitle, string blogId)
        {
            PostLink = postLink;
            BlogTitle = blogTitle;
            BlogId = blogId;
        }
    }
}
