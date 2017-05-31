using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Sql
{
    public class FromTable : FromBase
    {
        private string alias;
        private string tableName;

        public virtual string Alias
        {
            get { return this.alias; }
            set { this.alias = value; }
        }

        public virtual string TableName
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }

        public override string ToPhrase()
        {
            StringBuilder phrase = new StringBuilder();

            phrase.Append(tableName);

            if (!string.IsNullOrEmpty(alias))
            {
                phrase.Append(" as ");
                phrase.Append(alias);
            }

            return phrase.ToString();
        }

    }
}
