using System;
using System.Linq;

class EmptyLongestStr
{
    static void Main()
    {
        string input = "djkfdbcdkkk";
        string longestConsecutiveString = FindLongestConsecutiveSubstring(input);
        Console.WriteLine("The longest consecutive substring is: " + longestConsecutiveString);
    }

    public static string FindLongestConsecutiveSubstring(string str)
    {
        int maxLen = 0;
        string longest = "";
        int start = 0;

        for (int i = 0; i < str.Length; i++)
        {
            int len = 1; // 当前子串的长度
            char prevChar = char.ToLower(str[i]); // 转换为小写以便比较

            // 检查后面的字符是否形成连续子串
            for (int j = i + 1; j < str.Length; j++)
            {
                if (str[j] - prevChar == 1)
                {
                    len++;
                    prevChar = str[j];
                }
                else
                {
                    break;
                }
            }

            // 如果当前子串长度大于已知的最大长度，则更新最大长度和最长子串
            if (len > maxLen)
            {
                maxLen = len;
                longest = str.Substring(i, len);
            }
        }

        return longest;
    }
}
