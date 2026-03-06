using System;
using System.Collections.Generic;

namespace ScanApp.Common
{
    static public class GenericMediator
    {
        static IDictionary<string, List<Action>> pl_dict = new Dictionary<string, List<Action>>();

        static public void Register(string token, Action callback)
        {
            if (!pl_dict.ContainsKey(token))
            {
                var list = new List<Action>();
                list.Add(callback);
                pl_dict.Add(token, list);
            }
            else
            {
                bool found = false;
                foreach (var item in pl_dict[token])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                if (!found)
                    pl_dict[token].Add(callback);
            }
        }

        static public void Unregister(string token)
        {
            if (pl_dict.ContainsKey(token))
                pl_dict[token].Clear();
        }

        static public void Notify(string token)
        {
            if (pl_dict.ContainsKey(token))
                foreach (var callback in pl_dict[token])
                    callback();
        }
    }
}
