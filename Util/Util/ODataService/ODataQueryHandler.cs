
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Util.BaseFeatures;

namespace Util.Util.ODataService
{
    public class ODataQueryHandler<T>(IHttpContextAccessor _httpContextAccessor) :IODataQueryHandler<T> where T : class
    {
        public (List<T>, Paging) ApplyODataQuery(IQueryable<T> query, BaseQueryRequestModel request)
        {
            var queryOptions = QueryOptions.BuildODataQueryOptions<T>(request, _httpContextAccessor);

            if (queryOptions.Filter != null)
            {
                query = (IQueryable<T>)queryOptions.Filter.ApplyTo(query, new ODataQuerySettings());
            }

            int totalRecords = query.Count();

            query = (IQueryable<T>)queryOptions.ApplyTo(query);
            var paging = new Paging
            {
                totalItems = totalRecords,
                totalPages = (int)Math.Ceiling((double)totalRecords / request.pageSize),
                pageSize = request.pageSize,
                pageNumber = request.pageNumber
            };
            var list = query.ToList();
            return (list, paging);
        }

        public async Task<(List<T>, Paging)> ApplyODataQueryAsync(IQueryable<T> query, BaseQueryRequestModel request)
        {
            var queryOptions = QueryOptions.BuildODataQueryOptions<T>(request, _httpContextAccessor);

            if (queryOptions.Filter != null)
            {
                query = (IQueryable<T>)queryOptions.Filter.ApplyTo(query, new ODataQuerySettings());
            }

            int totalRecords =await  query.CountAsync();

            query = (IQueryable<T>)queryOptions.ApplyTo(query);
            var paging = new Paging
            {
                totalItems = totalRecords,
                totalPages = (int)Math.Ceiling((double)totalRecords / request.pageSize),
                pageSize = request.pageSize,
                pageNumber = request.pageNumber
            };
            var list = await query.ToListAsync();
            return (list, paging);
        }
    }
}
