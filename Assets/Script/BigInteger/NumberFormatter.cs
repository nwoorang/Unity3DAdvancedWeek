using System;
using System.Numerics;

public class NumberFormatter
{
    static string[] units = {
        "", "K", "M", "B", "T", 
        "aa", "ab", "ac", "ad", "ae",
        "af", "ag", "ah", "ai", "aj"
        // 필요하면 계속 추가
    };

    public static string Format(BigInteger number)
    {
        if (number < 1000)
            return number.ToString();

        int index = 0;
        while (number >= 1000)
        {
            number /= 1000;
            index++;
        }

        return number.ToString() + units[index];
    }
}
