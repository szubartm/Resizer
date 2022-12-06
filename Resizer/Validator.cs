namespace Resizer
{
    public class Validator
    {
        public static string Validate(string text)
        {
            if (!text.StartsWith("."))
            {
                return text.Insert(0, ".");
            }
            else
            {
                return text;
            }
        }
    }
}
