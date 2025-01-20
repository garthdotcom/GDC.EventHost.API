namespace GDC.EventHost.API.Helpers
{
    public static class StringExtensions
    {
        public static string FormatForSearch(this string str) 
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Replace('+', ' ');
        }
    }
}
