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
            int len = 1; // ��ǰ�Ӵ��ĳ���
            char prevChar = char.ToLower(str[i]); // ת��ΪСд�Ա�Ƚ�

            // ��������ַ��Ƿ��γ������Ӵ�
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

            // �����ǰ�Ӵ����ȴ�����֪����󳤶ȣ��������󳤶Ⱥ���Ӵ�
            if (len > maxLen)
            {
                maxLen = len;
                longest = str.Substring(i, len);
            }
        }

        return longest;
    }
}
