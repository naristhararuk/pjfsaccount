using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.GL.DAL
{
    public class GlDaoProvider
    {
        public GlDaoProvider() { }

        public static IGlAccountDao     GlAccountDao        { get; set; }
        public static IGlAccountLangDao GlAccountLangDao    { get; set; }
    }
}
