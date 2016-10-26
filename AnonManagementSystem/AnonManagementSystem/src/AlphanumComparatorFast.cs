using System;
using System.Collections.Generic;
using EquipmentInformationData;

namespace AnonManagementSystem
{
    public class AlphanumComparatorFast : IComparer<CombatEquipment>, IComparer<SparePart>
    {
        private static List<string> GetList(string s1)
        {
            List<string> sb1 = new List<string>();
            string st1;
            st1 = "";
            bool flag = char.IsDigit(s1[0]);
            foreach (char c in s1)
            {
                if (flag != char.IsDigit(c) || c == '\'')
                {
                    if (st1 != "")
                        sb1.Add(st1);
                    st1 = "";
                    flag = char.IsDigit(c);
                }
                if (char.IsDigit(c))
                {
                    st1 += c;
                }
                if (char.IsLetter(c))
                {
                    st1 += c;
                }
            }
            sb1.Add(st1);
            return sb1;
        }

        private int Compare(string x, string y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            if (x == y)
            {
                return 0;
            }

            // Walk through two the strings with two markers.
            List<string> str1 = GetList(x);
            List<string> str2 = GetList(y);
            while (str1.Count != str2.Count)
            {
                if (str1.Count < str2.Count)
                {
                    str1.Add("");
                }
                else
                {
                    str2.Add("");
                }
            }
            int x1 = 0;
            int x2 = 0;
            //s1status ==false then string ele int;
            //s2status ==false then string ele int;
            int result = 0;
            for (int i = 0; i < str1.Count && i < str2.Count; i++)
            {
                int res;
                int.TryParse(str1[i], out res);
                var s1Status = false;
                if (res != 0)
                {
                    x1 = Convert.ToInt32(str1[i]);
                    s1Status = true;
                }

                int.TryParse(str2[i], out res);
                var s2Status = false;
                if (res != 0)
                {
                    x2 = Convert.ToInt32(str2[i]);
                    s2Status = true;
                }
                //checking --the data comparision
                if (!s2Status && !s1Status)    //both are strings
                {
                    result = string.Compare(str1[i], str2[i], StringComparison.Ordinal);
                }
                else if (s2Status && s1Status) //both are intergers
                {
                    if (x1 == x2)
                    {
                        if (str1[i].Length < str2[i].Length)
                        {
                            result = 1;
                        }
                        else if (str1[i].Length > str2[i].Length)
                            result = -1;
                        else
                            result = 0;
                    }
                    else
                    {
                        int st1ZeroCount = str1[i].Trim().Length - str1[i].TrimStart('0').Length;
                        int st2ZeroCount = str2[i].Trim().Length - str2[i].TrimStart('0').Length;
                        if (st1ZeroCount > st2ZeroCount)
                            result = -1;
                        else if (st1ZeroCount < st2ZeroCount)
                            result = 1;
                        else
                            result = x1.CompareTo(x2);

                    }
                }
                else
                {
                    result = string.Compare(str1[i], str2[i], StringComparison.Ordinal);
                }
                if (result != 0)
                    break;
            }
            return result;
        }

        public int Compare(CombatEquipment x, CombatEquipment y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            return Compare(x.SerialNo, y.SerialNo);
        }

        public int Compare(SparePart x, SparePart y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            return Compare(x.SerialNo, y.SerialNo);
        }
    }
}
