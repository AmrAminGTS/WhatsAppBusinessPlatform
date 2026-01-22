using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using System.Reflection;

namespace WhatsAppBusinessPlatform.Infrastucture.Persistence.Configurations;
public static class ModelBuilderExtensions
{
    public static void ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        string isDeletedPropertyName = "IsDeleted";
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            // IMPORTANT: only apply filter if 'IsDeleted' is already a mapped property
            // (this respects any builder.Ignore(...) calls in entity configs)
            IMutableProperty? mappedProp = entityType.FindProperty(isDeletedPropertyName);
            if (mappedProp == null)
            {
                continue; // skip ProductView or any entity that ignored the property
            }

            // Get EntityTypeBuilder for the CLR type (non-generic overload)
            EntityTypeBuilder entityBuilder = modelBuilder.Entity(entityType.ClrType);

            // Ensure column exists (if the CLR property exists it maps; this is safe)
            // Using shadow property ensures HasDefaultValue even if property wasn't discovered for some reason
            entityBuilder.Property<bool>("IsDeleted").HasDefaultValue(false);

            // Build lambda: e => !EF.Property<bool>(e, "IsDeleted")
            ParameterExpression parameter = Expression.Parameter(entityType.ClrType, "e");
            MethodInfo efPropertyMethod = typeof(EF).GetMethod(nameof(EF.Property))!
                .MakeGenericMethod(typeof(bool));
            MethodCallExpression efPropertyCall = Expression.Call(efPropertyMethod, parameter, Expression.Constant(isDeletedPropertyName));
            UnaryExpression body = Expression.Not(efPropertyCall);
            LambdaExpression lambda = Expression.Lambda(body, parameter);

            // Apply the query filter (HasQueryFilter accepts LambdaExpression)
            entityBuilder.HasQueryFilter(lambda);
        }
    }
}
