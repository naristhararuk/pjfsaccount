using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.GL.Query;

namespace SCG.GL.Query
{
    public class GlQueryProvider
    {
        public GlQueryProvider() { }

        public static IGlAccountQuery       GlAccountQuery      { get; set; }
        public static IGlAccountLangQuery   GlAccountLangQuery  { get; set; }
    }
}
