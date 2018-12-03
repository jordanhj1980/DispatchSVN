using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace DispatchApp
{
    public class RequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "不能为空值！");
            if (string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "不能为空字符串！");
            else if (check_other_char(value.ToString()))
            {
                return new ValidationResult(false, "不能含有特殊字符！");
            }
            return new ValidationResult(true, null);
        }

        private bool check_other_char(string str)
        {
            char[] arr = { '&', '\\', '/', '*', '>', '<', '@', '!' };
            for (var i = 0; i < arr.Count(); i++)
            {
                for (var j = 0; j < str.Count(); j++)
                {
                    if (arr[i] == str[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
