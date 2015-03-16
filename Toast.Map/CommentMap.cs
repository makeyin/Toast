using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Toast.DomainModel;

namespace Toast.Map
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Id(c => c.Id);
            Map(c => c.Email);
            Map(c => c.CommentDate);
            Map(c => c.Text);
            Map(c => c.Rating);
            References<BlogPost>(c => c.BlogPost);
        }
    }
}
