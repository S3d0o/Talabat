using System.Reflection.Metadata.Ecma335;

namespace E_Commerce.Factories
{
    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}