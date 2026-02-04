using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Dtos.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // This attribute ensures that the enum is serialized/deserialized as a string in JSON
    public enum ProductSortingOptions
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc
    }
}
