using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class PropertyOperand : OperandBase
    {
        private string alias;
        private string propertyName;

        public virtual string Alias
        {
            get { return alias; }
            set { this.alias = value; }
        }

        public virtual string PropertyName
        {
            get { return propertyName; }
            set { this.propertyName = value; }
        }

        public override string ToPhrase()
        {
            StringBuilder phrase = new StringBuilder();

            if (!string.IsNullOrEmpty(alias))
            {
                phrase.Append(alias);
                phrase.Append(".");
            }

            phrase.Append(propertyName);

            return phrase.ToString();
        }
    }
}
