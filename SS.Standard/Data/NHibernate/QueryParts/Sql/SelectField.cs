using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Sql
{
    public class SelectField : SelectBase
    {
        private string alias;
        private string fieldName;

        public virtual string Alias
        {
            get { return alias; }
            set { this.alias = value; }
        }

        public virtual string FieldName
        {
            get { return fieldName; }
            set { this.fieldName = value; }
        }

        public override string ToPhrase()
        {
            StringBuilder phrase = new StringBuilder();

            if (!string.IsNullOrEmpty(alias))
            {
                phrase.Append(alias);
                phrase.Append(".");
            }

            phrase.Append(fieldName);

            return phrase.ToString();
        }

    }
}
