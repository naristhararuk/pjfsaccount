using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Data.NHibernate.QueryParts.Hql
{
    public class HqlValueOperand : OperandBase, IParameterFillable
    {
        private Type operandType;
        private string parameterName;

        public virtual Type OperandType
        {
            get { return this.operandType; }
            set { this.operandType = value; }
        }

        public virtual string ParameterName
        {
            get { return this.parameterName; }
            set { this.parameterName = value; }
        }

        public override string ToPhrase()
        {
            return ":" + parameterName;
        }

    }
}
