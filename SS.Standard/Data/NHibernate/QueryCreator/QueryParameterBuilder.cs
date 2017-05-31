using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace SS.Standard.Data.NHibernate.QueryCreator
{
    class QueryParameterData
    {
        private string parameterName;
        private Type parameterType;
        private object parameterValue;

        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        public Type ParameterType
        {
            get { return parameterType; }
            set { parameterType = value; }
        }

        public object ParameterValue
        {
            get { return parameterValue; }
            set { parameterValue = value; }
        }
    }

    public class QueryParameterBuilder
    {
        IList<QueryParameterData> listQueryParameterData = new List<QueryParameterData>();

        public void AddParameterData(string parameterName, Type parameterType, object parameterValue)
        {
            QueryParameterData queryParameterData = new QueryParameterData();
            queryParameterData.ParameterName = parameterName;
            queryParameterData.ParameterType = parameterType;
            queryParameterData.ParameterValue = parameterValue;
            listQueryParameterData.Add(queryParameterData);
        }

        public void FillParameters(IQuery query)
        {
            foreach (QueryParameterData queryParameterData in listQueryParameterData)
            {
                FillParameter(query, queryParameterData.ParameterName, queryParameterData.ParameterType, queryParameterData.ParameterValue);
            }
        }

        public void FillParameters(ISQLQuery sqlQuery)
        {
            foreach (QueryParameterData queryParameterData in listQueryParameterData)
            {
                FillParameter(sqlQuery, queryParameterData.ParameterName, queryParameterData.ParameterType, queryParameterData.ParameterValue);
            }
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
