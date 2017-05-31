using System;
using System.Collections.Generic;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.QueryParts.Constants;
using SS.Standard.Data.NHibernate.QueryParts;

namespace SS.Standard.Data.NHibernate.QueryCreator
{
    public class HqlCreator : QueryCreatorBase
    {
        public void FillParameters(IQuery query, QueryPartCollector queryPartCollector)
        {
            IList<QueryPartCollector.CriterionValuePair> criterionValuePairs = queryPartCollector.CriterionValuePairs;
            IDictionary<Object, CriterionBase> criterionParts = queryDescriptor.CriterionParts;

            foreach (QueryPartCollector.CriterionValuePair criterionValuePair in criterionValuePairs)
            {
                CriterionBase criterionBase = (CriterionBase)criterionParts[criterionValuePair.CriterionKey];

                if (criterionBase.IsCriterionParameterRequired())
                {
                    if (criterionBase is ISingleParameterCriterion)
                    {
                        ISingleParameterCriterion singleParameterCriterion = (ISingleParameterCriterion)criterionBase;
                        string criterionParameterName = singleParameterCriterion.CriterionParameterName;
                        Type criterionParameterType = singleParameterCriterion.CriterionParameterType;

                        FillParameter(query, criterionParameterName, criterionParameterType, GetCriterionValue(criterionBase, criterionValuePair));
                    }
                }
            }
        }

        protected object GetCriterionValue(CriterionBase criterionBase, QueryPartCollector.CriterionValuePair criterionValuePair)
        {
            object criterionValue = criterionValuePair.CriterionValue;

            if (criterionBase is IPatternMatchCriterion)
            {
                IPatternMatchCriterion patternMatchCriterion = (IPatternMatchCriterion)criterionBase;
                string strCriterionValue = Convert.ToString(criterionValue);

                if (MatchOption.End == patternMatchCriterion.MatchOption)
                {
                    criterionValue = "%" + strCriterionValue;
                }
                else if (MatchOption.Exact == patternMatchCriterion.MatchOption)
                {
                    criterionValue = strCriterionValue;
                }
                else if (MatchOption.Start == patternMatchCriterion.MatchOption)
                {
                    criterionValue = strCriterionValue + "%";
                }
                else
                {
                    criterionValue = "%" + strCriterionValue + "%";
                }
            }

            return criterionValue;
        }

        protected void FillParameter(IQuery query, string criterionParameterName, Type criterionParameterType, object criterionParamterValue)
        {
            if (typeof(string) == criterionParameterType)
            {
                query.SetString(criterionParameterName, Convert.ToString(criterionParamterValue));
            }
            else if (typeof(int) == criterionParameterType)
            {
                query.SetInt32(criterionParameterName, Convert.ToInt32(criterionParamterValue));
            }
            else if (typeof(long) == criterionParameterType)
            {
                query.SetInt64(criterionParameterName, Convert.ToInt64(criterionParamterValue));
            }
            else if (typeof(short) == criterionParameterType)
            {
                query.SetInt16(criterionParameterName, Convert.ToInt16(criterionParamterValue));
            }
            else if (typeof(decimal) == criterionParameterType)
            {
                query.SetDecimal(criterionParameterName, Convert.ToDecimal(criterionParamterValue));
            }
            else if (typeof(double) == criterionParameterType)
            {
                query.SetDouble(criterionParameterName, Convert.ToDouble(criterionParamterValue));
            }
            else if (typeof(float) == criterionParameterType)
            {
                query.SetDouble(criterionParameterName, Convert.ToDouble(criterionParamterValue));
            }
            else if (typeof(char) == criterionParameterType)
            {
                query.SetCharacter(criterionParameterName, Convert.ToChar(criterionParamterValue));
            }
            else if (typeof(bool) == criterionParameterType)
            {
                query.SetBoolean(criterionParameterName, Convert.ToBoolean(criterionParamterValue));
            }
            else if (typeof(DateTime) == criterionParameterType)
            {
                query.SetDateTime(criterionParameterName, Convert.ToDateTime(criterionParamterValue));
            }
        }

    }
}
