using System.Text.Json.Serialization;

namespace WebApi.Contexts;

[JsonSerializable(typeof(int))]
internal sealed partial class OpenApiJsonSerializerContext : JsonSerializerContext
{
}
