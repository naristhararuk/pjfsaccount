using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Security.DAL.Hibernate;

namespace SS.Standard.Security.DAL
{
    public class DaoProvider
    {
        public DaoProvider() { }
        public static UserDao UserDao { get; set; }

    }
}