using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace sk.core.Filters
{
    public class AuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            foreach (var attr in attrs)
            {
                // 如果 Attribute 是我们自定义的验证过滤器
                if (attr.Filter.GetType() == typeof(MemberParamterFilter))
                {
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "MemberToken",
                        In = "header",
                        Type = "string",
                        Required = true,
                        Description = "登录验证的Token"
                    });
                }
            }
        }
    }
}
