using System.Text.RegularExpressions;

namespace UcabGo.App.Utils;

public static class StringHelper
{
    public static bool IsValidPhone(this string phone)
    {
        return Regex.IsMatch(phone, @"^\s*(?:(?:\+58)(?:-)?(?:4(?:14|24|12||26))|(?:0(?:414|424|412|416|426)))[-]?[0-9]{7}\s*$");
    }

    public static string FormatPhone(this string phone)
    {
        //Delete non numeric characters
        phone = Regex.Replace(phone.Trim(), "[^0-9]", "");

        //Add a 0 at the beginning if it doesn't have it
        if (phone[0] != '0') phone = string.Concat("0", phone);

        return phone;
    }


    public static bool IsValidEmail(this string email)
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@(?:[a-zA-Z0-9-]+\.)?ucab\.edu\.ve$";
        return Regex.IsMatch(email, pattern);
    }

    public static bool IsValidPassword(this string password)
    {
        var pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        return Regex.IsMatch(password, pattern);
    }
}
