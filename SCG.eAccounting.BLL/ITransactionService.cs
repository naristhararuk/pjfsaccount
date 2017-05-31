using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.BLL
{
	public interface ITransactionService
	{
		Guid Begin(DataSet ds);
		Guid Begin(Guid parentTxID);
		void Commit(Guid TxID);
		void Rollback(Guid TxID);
		DataSet GetDS(Guid TxID);
	}
}
