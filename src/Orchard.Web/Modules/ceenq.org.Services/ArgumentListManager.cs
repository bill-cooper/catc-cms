using System;
using System.Collections.Generic;
using System.Text;

namespace ceenq.org.Services
{
    internal static class ArgumentListManager
    {
        internal static void AddArgument(Dictionary<String, String> dictionary, String name, Object value)
        {
            if (value is Boolean)
            {
                dictionary.Add(name, (Boolean) value ? "1" : "0");
            }
            else
            {
                dictionary.Add(name, value.ToString());
            }
        }

        internal static void ApplyArguments(Dictionary<String, String> dictionary, IDefaultOptimization settings)
        {
            Dictionary<String, String> settingsList = settings.GetSettingsList();
            foreach (String key in settingsList.Keys)
            {
                dictionary.Add(key, settingsList[key]);
            }
        }

        internal static String GetArgumentString(Dictionary<String, String> dictionary)
        {
            var b = new StringBuilder();
            foreach (String key in dictionary.Keys)
            {
                if (!String.IsNullOrEmpty(dictionary[key]))
                {
                    b.Append(String.Format("&{0}={1}", key, dictionary[key]));
                }
            }
            return b.ToString();
        }

        internal static void InitArgumentList(Dictionary<String, String> dictionary, String key)
        {
            dictionary.Add("key", key);
        }
    }
}