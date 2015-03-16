using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Toast.DomainModel;

namespace Toast.Map
{
    public class BlogPostMap : ClassMap<BlogPost>
    {
        public BlogPostMap()
        {
            Id(p => p.Id);
            Map(p => p.Title);
            Map(p => p.SubTitle);
            Map(p => p.Text);
            Map(p => p.PublicationDate);
            Map(p => p.AuthorName);
            HasMany<Comment>(p => p.Comments)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }
}
