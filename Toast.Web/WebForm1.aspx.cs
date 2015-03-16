using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using Toast.Factory;
using Toast.DomainModel;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;

namespace Toast.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private ISession session = null;
        public ISessionFactory sessionFactory = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            //foreach (BlogPost c in list2)
            //{
            //    session.Save(c);
            //}
              
            //reposoitory.(post);//  SessionBuilder.OpenSession();
            //SessionBuilder.
            // session.Save(post);


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SessionBuilder.Instance("call", "DbCfg");
            Repository<BlogPost> reposoitory = new Repository<BlogPost>();
            //   IList<BlogPost> list = reposoitory.GetAll();
            IList<BlogPost> list2 = new List<BlogPost>();
            int a = reposoitory.GetCount(" from BlogPost");

            BlogPost post = new BlogPost();
            //   session=sessionFactory.OpenSession();

            post.AuthorName = "111";
            post.PublicationDate = DateTime.Now;
            post.Text = "232";
            post.Title = "wew";
            post.SubTitle = "wwewsssss";
            Comment m = new Comment() { CommentDate = DateTime.Now, Rating = 2, Email = "wewesss", Text = "222" };
            //post.AddCommant(m);
            reposoitory.Create(post);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            SessionBuilder.Instance("call");

            Repository<BlogPost> reposoitory = new Repository<BlogPost>();
            //   IList<BlogPost> list = reposoitory.GetAll();
            IList<BlogPost> list2 = new List<BlogPost>();
            int a = reposoitory.GetCount(" from BlogPost");

            BlogPost post = new BlogPost();
            //   session=sessionFactory.OpenSession();

            post.AuthorName = "111";
            post.PublicationDate = DateTime.Now;
            post.Text = "232";
            post.Title = "wew";
            post.SubTitle = "wwewsssss";
            Comment m = new Comment() { CommentDate = DateTime.Now, Rating = 2, Email = "wewesss", Text = "222" };
            //post.AddCommant(m);
            reposoitory.Create(post);


        }


    }
}