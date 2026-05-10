using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Util.Util.BaseFeatures;

namespace Util.Util
{
    public static class QueryOptions
    {
        public static ODataQueryOptions<TEntity> BuildODataQueryOptions<TEntity>(BaseQueryRequestModel request, IHttpContextAccessor httpContextAccessor)
        {
            const int MinRecordsPerPage = 1; // Define the minimum records per page
            const int MaxRecordsPerPage = 100; // Define the maximum 
            var modelBuilder = new ODataConventionModelBuilder();

            RegisterEntities(modelBuilder, typeof(TEntity));

            IEdmModel model = modelBuilder.GetEdmModel();
            var context = new ODataQueryContext(model, typeof(TEntity), new ODataPath());

            // Create the query string from the request model
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (request.filter != null)
            {
                queryString["$filter"] = request.filter;
            }

            // Add sorting
            if (request.orderby != null)
            {
                queryString["$orderby"] = request.orderby;
            }

            // Add pagination
            int recordsPerPage = request.pageSize!;
            if (recordsPerPage < MinRecordsPerPage)
            {
                recordsPerPage = MinRecordsPerPage;
            }
            else if (recordsPerPage > MaxRecordsPerPage)
            {
                recordsPerPage = MaxRecordsPerPage;
            }
            queryString["$skip"] = ((request.pageNumber - 1) * recordsPerPage).ToString();
            queryString["$top"] = recordsPerPage.ToString();

            var requestMessage = httpContextAccessor.HttpContext?.Request;
            requestMessage.QueryString = new QueryString("?" + queryString.ToString());

            return new ODataQueryOptions<TEntity>(context, requestMessage);
        }

        private static void RegisterEntities(ODataConventionModelBuilder modelBuilder, Type mainEntityType)
        {
            var entityTypes = new HashSet<Type> { mainEntityType };
            var toProcess = new Queue<Type>();
            toProcess.Enqueue(mainEntityType);

            while (toProcess.Count > 0)
            {
                var currentType = toProcess.Dequeue();
                var properties = currentType.GetProperties();

                foreach (var property in properties)
                {
                    if (property.PropertyType.IsClass && property.PropertyType.Namespace == mainEntityType.Namespace && !entityTypes.Contains(property.PropertyType))
                    {
                        entityTypes.Add(property.PropertyType);
                        toProcess.Enqueue(property.PropertyType);
                    }
                }
            }

            foreach (var entityType in entityTypes)
            {
                modelBuilder.AddEntityType(entityType);
            }
        }
    }
}