using System.Text;

namespace Feedro.Model.Helper
{
    public static class StringHelper
    {
        private enum ParsingMode
        {
            Text,
            Tag,
            Escape
        }

        public static string RemoveHtmlTags(this string text)
        {
            var result = new StringBuilder();
            var buffer = new StringBuilder();
            var mode   = ParsingMode.Text;

            foreach (char ch in text)
            {
                switch (mode)
                {
                    case ParsingMode.Text:

                        switch (ch)
                        {
                            case '<':
                                mode = ParsingMode.Tag;
                                break;
                            case '&':
                                mode = ParsingMode.Escape;
                                buffer.Clear();
                                break;
                            default:
                                if (!char.IsControl(ch))
                                    result.Append(ch);
                                break;
                        }

                        break;

                    case ParsingMode.Tag:

                        if (ch == '>')
                            mode = ParsingMode.Text;

                        break;

                    case ParsingMode.Escape:

                        switch (ch)
                        {
                            case ';':
                                switch (buffer.ToString().ToLower())
                                {
                                    case "nbsp":
                                        result.Append(' ');
                                        break;
                                    case "lt":
                                        result.Append('<');
                                        break;
                                    case "gt":
                                        result.Append('>');
                                        break;
                                    default:
                                        // Ignore other escape sequendes
                                        break;
                                }
                                break;
                            default:
                                buffer.Append(ch);
                                break;
                        }

                        break;
                }
            }

            return result.ToString();
        }
    }
}
