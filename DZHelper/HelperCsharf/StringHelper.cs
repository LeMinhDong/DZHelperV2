
using System.Web;

namespace DZHelper.HelperCsharf
{
    public static class StringHelper
    {
        public static List<string> ParseLines(this string content, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
			if (content == null) return new List<string>();
            return content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string EscapeString(this string input)
        {
            //command trong cmd
            Dictionary<char, string> escapeChars = new Dictionary<char, string>
            {
                { ' ', "%s" },
                { '&', "\\&" },
                { '<', "\\<" },
                { '>', "\\>" },
                { '?', "\\?" },
                { ':', "\\:" },
                { '{', "\\{" },
                { '}', "\\}" },
                { '[', "\\[" },
                { ']', "\\]" },
                { '|', "\\|" }
            };

            foreach (var escapeChar in escapeChars)
            {
                input = input.Replace(escapeChar.Key.ToString(), escapeChar.Value);
            }

            return input;
        }

        public static string ConvertToSpacedStringManual(this string input)
        {
            return string.Concat(input.Select((c, i) => i > 0 && char.IsUpper(c) ? " " + c : c.ToString()));
        }

    }
}
