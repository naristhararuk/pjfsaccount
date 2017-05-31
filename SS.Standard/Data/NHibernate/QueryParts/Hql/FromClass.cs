using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class FromClass : FromBase
    {
        private string alias;
        private Type classType;

        public virtual string Alias
        {
            get { return this.alias; }
            set { this.alias = value; }
        }

        public virtual Type ClassType
        {
            get { return this.classType; }
            set { this.classType = value; }
        }

        public override string ToPhrase()
        {
            StringBuilder phrase = new StringBuilder();

            phrase.Append(classType.Name);

            if (!string.IsNullOrEmpty(alias))
            {
                phrase.Append(" as ");
                phrase.Append(alias);
            }

            return phrase.ToString();
        }
    }
}
