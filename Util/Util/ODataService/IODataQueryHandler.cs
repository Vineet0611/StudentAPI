using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Util.BaseFeatures;

namespace Util.Util.ODataService
{
    public interface IODataQueryHandler <T> where T : class
    {
        (List<T>, Paging) ApplyODataQuery(IQueryable<T> query, BaseQueryRequestModel request);
        Task<(List<T>, Paging)> ApplyODataQueryAsync(IQueryable<T> query, BaseQueryRequestModel request);
    }
}
