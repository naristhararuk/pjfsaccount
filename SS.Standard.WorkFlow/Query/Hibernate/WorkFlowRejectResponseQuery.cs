﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;

using SS.Standard.WorkFlow.DTO;

using NHibernate;
using NHibernate.Transform;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SS.Standard.WorkFlow.Query.Hibernate
{
    public class WorkFlowRejectResponseQuery : NHibernateQueryBase<DTO.WorkFlowRejectResponse, long>, IWorkFlowRejectResponseQuery
    {
        
    }
}
