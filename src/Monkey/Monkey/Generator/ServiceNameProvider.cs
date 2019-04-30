using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Humanizer;

namespace Monkey.Generator
{
    public interface IServiceNameProvider
    {
        string EvaluateHandler(HandlerInfo info);
    }
    public class ServiceNameProvider : IServiceNameProvider
    {
        private static void SingularizeLastWord(IList<string> words)
        {
            var last = words.Last();
            var s = last.Singularize();
            if (s != null && s != last)
            {
                words[words.Count - 1] = s;
            }
        }
        public string EvaluateHandler(HandlerInfo info)
        {
            var sn = info.HandlerIType.GetCustomAttribute<ServiceNameAttribute>() ??
                     info.HandlerType?.GetCustomAttribute<ServiceNameAttribute>();
            if (sn != null)
                return sn.ServiceName;

            string text = null;

            if (info.IsCommandHandler)
            {
                if (info.HandlerType != null && !info.HandlerType.Name.Contains("ObjectProxy")) // Heuristic
                    text = info.HandlerType.Name.RemoveSuffixes("CommandHandler", "Handler");
                else
                    text = info.RequestType.Name.RemoveSuffixWords("Command", "Request");


                var words = text.ToWords().ToList();
                SingularizeLastWord(words);

                if (CommandVerbs.Any(x => x == words[0]))
                {
                    words.RemoveAt(0);
                    return string.Concat(words);
                }

                if (words.Count > 1)
                    return string.Concat(words.Skip(1));
                return string.Concat(words);

            }
            else if (info.IsQueryHandler)
            {
                if (info.HandlerType != null && !info.HandlerType.Name.Contains("ObjectProxy")) // Heuristic
                    text = info.HandlerType.Name.RemoveSuffixes("QueryHandler", "Handler");
                else
                    text = info.RequestType.Name.RemoveSuffixWords("Query", "Request");

                var words = text.ToWords().ToList();
                SingularizeLastWord(words);

                if (words.Count > 3)
                {
                    if (words[0] == "Get" && words[1] == "By")
                        return string.Concat(words.Skip(3));
                }

                if (words[0] == "Get")
                {
                    var array = words.Skip(1).ToArray();
                    array[0] = array[0].Singularize();
                    return string.Concat(array);
                }

                return string.Concat(words);

            }
            throw new InvalidOperationException();
        }
        
        private string[] CommandVerbs = new string[] {"Add", "Update","Delete", "Remove", "Insert", "Create", "Modify","Edit", "Insert"};
    }
}