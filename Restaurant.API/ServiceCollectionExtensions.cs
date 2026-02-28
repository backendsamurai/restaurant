using Microsoft.OpenApi;

namespace Restaurant.API;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterOpenApiDefinition(string groupName, string title)
        {
            return services.AddOpenApi(groupName, opt =>
            {
                opt.ShouldInclude = (description) => description.GroupName == groupName;

                opt.AddDocumentTransformer((document, context, _) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = title,
                        Version = "v1",
                    };

                    return Task.CompletedTask;
                });
            });
        }
    }
}
