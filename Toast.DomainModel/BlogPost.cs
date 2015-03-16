using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toast.DomainModel
{
    public class BlogPost 
    {
        IList<Comment> _comments;
        public BlogPost()
        {
            _comments = new List<Comment>();
        }
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string SubTitle { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime PublicationDate { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual IList<Comment> Comments { get { return _comments.ToList().AsReadOnly(); } }

        public virtual void AddCommant(Comment comment)
        {
            comment.BlogPost = this;
            if (!_comments.Contains(comment))
                this._comments.Add(comment);
        }
    }
}
