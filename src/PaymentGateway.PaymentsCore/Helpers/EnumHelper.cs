using System;
using System.Linq;
using System.Reflection;

namespace PaymentGateway.PaymentsCore.Helpers
{
    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if (memberInfo.Length <= 0) return genericEnum.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attrs.Any() ? ((System.ComponentModel.DescriptionAttribute)attrs.ElementAt(0)).Description : genericEnum.ToString();
        }

    }
}