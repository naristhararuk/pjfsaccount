using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;
using SCG.DB.DTO.DataSet;

namespace SCG.DB.BLL
{
    public interface IDbPBService : IService<Dbpb, long>
    {
        long AddPB(Dbpb pb, DBPbDataSet pbDataSet);
        void UpdatePB(Dbpb pb, DBPbDataSet pbDataSet);
        void DeletePB(Dbpb pb);
    }
}
