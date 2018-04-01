namespace Bible.Helpers
{
    using System;
    using System.Text.RegularExpressions;

    public static class RegexUtilities
    {
        public static bool IsValidEmail(string email)
        {
            var expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool Reg_exp(string fieldValue)
        {
            string reg_exp = @"[^\d\:\-]";
            Regex auxRegex = new Regex(reg_exp);
            return auxRegex.IsMatch(fieldValue);
        }
    }
}
