using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharlieDontSurf.Helpers
{
    public static class Helper
    {
        public static string StripSpaces(string text)
        {
            int pos = text.Length - 1;

            while (pos >= 0 && text.Substring(pos, 1) == " ")
                pos--;

            return text.Substring(0, pos + 1);
        }
    }
}