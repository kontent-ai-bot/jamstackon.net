using System.Text.RegularExpressions;

namespace Jamstack.On.Dotnet.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string GenerateAnchorIdentifier(this string input)
        {
            return Regex.Replace(input.ToLower(), "[^a-z,0-9]+", "-");
        }
    }
}
