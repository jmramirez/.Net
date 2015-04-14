using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication5.Models;

namespace WebApplication5.ViewModels
{
    public class RepositoryBase
    {
        protected ApplicationDbContext dc;
        protected RepositoryBase()
        {
            dc = new ApplicationDbContext();
            dc.Configuration.ProxyCreationEnabled = false;
            dc.Configuration.LazyLoadingEnabled = false;
        }
    }
}