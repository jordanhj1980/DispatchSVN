using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchApp
{
    class GlobalFunAndVar
    {
        public static string sequenceGenerator()
        {
            StringBuilder seq = new StringBuilder(10);

            System.DateTime currentTime = System.DateTime.Now;
            string dateStr = currentTime.ToString("hhmmss");
            seq.Append(dateStr);

            int a = new Random().Next(0, 1000);
            seq.Append(string.Format("{0:0000}", a));

            return seq.ToString();
        }
    }
}
