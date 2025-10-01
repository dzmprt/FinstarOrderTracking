using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

public class NonNullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null || !context.Type.IsClass)
        {
            return;
        }

        foreach (var property in schema.Properties)
        {
            // Find the corresponding C# property
            var cSharpProperty = context.Type.GetProperty(property.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (cSharpProperty != null)
            {
                // Check if the C# property is a non-nullable value type or a non-nullable reference type
                // (requires nullable reference types to be enabled in your project)
                var isNonNullable = cSharpProperty.PropertyType.IsValueType && Nullable.GetUnderlyingType(cSharpProperty.PropertyType) == null ||
                                    (cSharpProperty.PropertyType.IsClass && cSharpProperty.CustomAttributes.All(a => a.AttributeType.Name != "NullableAttribute")); // Heuristic for NRT

                if (isNonNullable && !schema.Required.Contains(property.Key))
                {
                    schema.Required.Add(property.Key);
                }
            }
        }
    }
}