using Microsoft.AspNetCore.Http;

namespace Aspire.Hosting.ApplicationModel;

public sealed class ServiceBusResource(string name) : ContainerResource(name), IResourceWithConnectionString
{
    internal const string ServiceBusEndpointName = "servicebus";
    
    private EndpointReference? _serviceBusReference;
    
    public EndpointReference ServiceBusEndpoint => _serviceBusReference ??= new(this, ServiceBusEndpointName);
    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create(
            $"Endpoint=sb://{ServiceBusEndpoint.Property(EndpointProperty.Host)}:{ServiceBusEndpoint.Property(EndpointProperty.Port)};SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;");
}