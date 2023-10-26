using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LIT.Old_LIT
{
    public static class ValidationClass
    {

        public static bool IsTextAllowed(string Text)
        {
            try
            {
                var regex = new Regex(@"[^a-zA-Z]");
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }
        public static bool IsNumeric(string Text)
        {
            try
            {
                var regex = new Regex(@"[^0-9]");
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }
        public static bool IsNumericDot(string Text)
        {
            try
            {
                var regex = new Regex(@"[^0-9.]");
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }
        public static bool IsNumericDotwith2decimal(string Text)
        {
            try
            {                
                var regex = new Regex(@"[^\d+(\.\d{0,2})?]");
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }

        public static bool IsAlphaNumeric(string Text)
        {
            try
            {
                var regex = new Regex(@"[^a-zA-Z-0-9]");
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }
        public static bool IsAlphaNumericDot(string Text)
        {
            try
            {
                var regex = new Regex(@"[^a-zA-Z-0-9-.]");
                return !regex.IsMatch(Text);
            }
            catch
            {
                return true;
            }
        }
    }
}
