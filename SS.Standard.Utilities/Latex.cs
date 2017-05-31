using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Utilities
{
    public class Latex
    {
        public static string Escape(string latex)
        {
            if (latex != null)
                return latex.Replace("$", @"\$").Replace("^", "\textasciicircum{}").Replace("{", @"\{").Replace("}", @"\}").Replace(@"\", @"\textbackslash{}").Replace("%", @"\%").Replace("#", @"\#").Replace("_", @"\_");
            else
                return latex;
        }
    }
}
