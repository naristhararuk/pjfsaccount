using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.DTO;

namespace SS.Standard.Data.NHibernate.Dao
{
	public class NHibernateAdapter<DTO, PK> where DTO : INHibernateAdapterDTO<PK>, new()
	{
		public void UpdateChange(DataTable dt, IDao<DTO, PK> dao)
		{
			DataTable insertTable = dt.GetChanges(DataRowState.Added);
			DataTable updateTable = dt.GetChanges(DataRowState.Modified);
			DataTable deleteTable = dt.GetChanges(DataRowState.Deleted);

			if (deleteTable != null)
			{
				foreach (DataRow dr in deleteTable.Rows)
				{
					PK id = new DTO().GetIDFromDataRow(dr, DataRowVersion.Original);
					DTO obj = dao.FindProxyByIdentity(id);
					dao.Delete(obj);
				}
			}

			if (updateTable != null)
			{
				foreach (DataRow dr in updateTable.Rows)
				{
					PK id = new DTO().GetIDFromDataRow(dr);
					DTO obj = dao.FindProxyByIdentity(id);
					obj.LoadFromDataRow(dr);
					dao.SaveOrUpdate(obj);
				}
			}

			if (insertTable != null)
			{
				foreach (DataRow dr in insertTable.Rows)
				{
					DTO obj = new DTO();
					obj.LoadFromDataRow(dr);
					PK newID = dao.Save(obj);
					obj.SaveIDToDataRow(dt, dr, newID);
				}
			}
		}
	}
}
