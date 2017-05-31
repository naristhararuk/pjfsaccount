using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Data.NHibernate.DTO
{
	public interface INHibernateAdapterDTO<PK>
	{
		void LoadFromDataRow(DataRow dr);
		void SaveIDToDataRow(DataTable dt, DataRow dr, PK newID);
		PK GetIDFromDataRow(DataRow dr);
		PK GetIDFromDataRow(DataRow dr, DataRowVersion rowState);
	}
}