using System;
using System.Text;
using Orchard;

namespace ceenq.com.AppRoutingServer.Services
{
    public interface INginxConfigPrettyFormatter : IDependency
    {
        string Format(string text);
    }
    public class NginxConfigPrettyFormatter : INginxConfigPrettyFormatter
    {
        public string Format(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.Replace(System.Environment.NewLine, string.Empty).Replace("\t", string.Empty);

            var offset = 0;
            var output = new StringBuilder();
            Action<StringBuilder, int> tabs = (sb, pos) => { for (var i = 0; i < pos; i++) { sb.Append("\t"); } };

            for (var i = 0; i < text.Length; i++)
            {
                var chr = text[i];
                if (chr == '{')
                {
                    offset++;
                    output.Append(chr);
                    output.Append(Environment.NewLine);
                    tabs(output, offset);
                }
                else if (chr == '}')
                {
                    offset--;
                    output.Append(chr);
                    output.Append(Environment.NewLine);
                    tabs(output, offset);
                }
                else if (chr == ';')
                {
                    output.Append(chr);
                    //in some cases, the semicolon is used within a directive as a delimiter.
                    //in such cases we expect the semicolon to be followed by a space
                    if (i + 1 < text.Length && text[i + 1] != ' ')
                    {
                        output.Append(Environment.NewLine);
                        tabs(output, offset);
                    }
                }
                else
                    output.Append(chr);
            }

            return output.ToString().Trim();
        }
    }
}
