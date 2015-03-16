using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toast.DomainModel
{
    public class Comment 
    {
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime CommentDate { get; set; }
        public virtual string Text { get; set; }
        public virtual int Rating { get; set; }
        public virtual BlogPost BlogPost { get; set; }
    }
}
