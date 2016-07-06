using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACA2
{
    public static class TextManipulation
    {
        
        public static string StripDashesAndTrim(this string str)
        {
            // this method will remove all dashes in the text
            // and it will remove spaces before and after the text
            return str.Trim().Replace("-", String.Empty);
        }

    }
}
