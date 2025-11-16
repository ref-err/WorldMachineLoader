using System;

namespace WorldMachineLoader.Utils
{
    public static class VersionUtils
    {
        public static int Compare(string v1, string v2)
        {
            Version n1; string t1;
            Version n2; string t2;

            Split(v1, out n1, out t1);
            Split(v2, out n2, out t2);

            int numericCompare = n1.CompareTo(n2);
            if (numericCompare != 0)
                return numericCompare;

            return CompareTags(t1, t2);
        }

        public static void Split(string input, out Version numeric, out string tag)
        {
            var parts = input.Split(new[] { '-' }, 2);

            string num = parts[0];
            tag = parts.Length > 1 ? parts[1] : "";

            if (num.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                num = num.Substring(1);
            }

            string[] nums = num.Split('.');
            while (nums.Length < 3)
                num += ".0";

            numeric = new Version(num);
        }

        public static int CompareTags(string t1, string t2)
        {
            bool e1 = string.IsNullOrEmpty(t1);
            bool e2 = string.IsNullOrEmpty(t2);

            if (e1 && e2) return 0;
            if (e1) return 1;
            if (e2) return -1;

            string[] order = { "alpha", "beta", "rc" };

            int i1 = IndexOf(order, t1.ToLower());
            int i2 = IndexOf(order, t2.ToLower());

            return i1.CompareTo(i2);
        }

        public static int IndexOf(string[] arr, string v)
        {
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == v)
                    return i;

            return int.MaxValue;
        }
    }
}
