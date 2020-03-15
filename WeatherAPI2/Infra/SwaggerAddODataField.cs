using Microsoft.AspNet.OData;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Reflection;

namespace WeatherAPI2.Infra
{
    public class SwaggerAddODataField : IOperationFilter
    {
        private const string OPERATION_DESCRIPTION = @"We allow the following OData operations:
<a target='_blank' href='http://docs.oasis-open.org/odata/odata/v4.01/cs01/part1-protocol/odata-v4.01-cs01-part1-protocol.html#sec_SystemQueryOptiontop'>Top</a> |
<a target='_blank' href='http://docs.oasis-open.org/odata/odata/v4.01/cs01/part1-protocol/odata-v4.01-cs01-part1-protocol.html#sec_SystemQueryOptionskip'>Skip</a> |
<a target='_blank' href='http://docs.oasis-open.org/odata/odata/v4.01/cs01/part1-protocol/odata-v4.01-cs01-part1-protocol.html#sec_SystemQueryOptionorderby'>OrderBy</a> |
<a target='_blank' href='http://docs.oasis-open.org/odata/odata/v4.01/cs01/part1-protocol/odata-v4.01-cs01-part1-protocol.html#sec_SystemQueryOptionfilter'>Filter</a> |
<a target='_blank' href='http://docs.oasis-open.org/odata/odata/v4.01/cs01/part1-protocol/odata-v4.01-cs01-part1-protocol.html#sec_SystemQueryOptioncount'>Count</a>";

        private static readonly OpenApiParameter TopParameter = CreateODataParameter("number", "$top", "OData: Field that allow you to set how many registers will be returned(max 30)", "10");
        private static readonly OpenApiParameter SkipParameter = CreateODataParameter("number", "$skip", "OData: Field that allow to skip registers", "5");
        private static readonly OpenApiParameter OrderByParameter = CreateODataParameter("string", "$orderBy", "OData: Field that allow to order data", "fieldName desc, fieldName2 asc");
        private static readonly OpenApiParameter FilterParameter = CreateODataParameter("string", "$filter", "OData: Field that allow to filter data", "fieldName eq 'Some string' and intField eq 2");
        private static readonly OpenApiParameter CountParameter = CreateODataParameter("boolean", "$count", "OData: Field that allow to set if you want the total count", "true");
        private static readonly OpenApiParameter[] ODataParameters = { FilterParameter, OrderByParameter, SkipParameter, TopParameter, CountParameter };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttribute<EnableQueryAttribute>() == null)
                return;

            operation.Description = OPERATION_DESCRIPTION;

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>(ODataParameters);
            else
                foreach (var p in ODataParameters)
                    operation.Parameters.Add(p);
        }

        private static OpenApiParameter CreateODataParameter(string type, string name, string description, string example)
            => new OpenApiParameter
            {
                Schema = new OpenApiSchema() { Type = type },
                Name = name,
                Description = description,
                Required = false,
                In = ParameterLocation.Query
            };
    }
}
