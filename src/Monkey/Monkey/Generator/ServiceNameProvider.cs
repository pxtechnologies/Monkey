using System;
using System.Linq;
using System.Reflection;

namespace Monkey.Generator
{
    public interface IServiceNameProvider
    {
        string EvaluateHandler(HandlerInfo info);
    }
    public class ServiceNameProvider
    {
        public string EvaluateHandler(HandlerInfo info)
        {
            if (info.IsCommandHandler)
            {
                var sn = info.HandlerType.GetCustomAttribute<ServiceNameAttribute>();
                if (sn != null)
                    return sn.ServiceName;
                string text = info.RequestType.Name.Replace("CommandHandler", "");
                var words = text.ToWords().ToList();
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
                var sn = info.HandlerType.GetCustomAttribute<ServiceNameAttribute>();
                if (sn != null)
                    return sn.ServiceName;

                string text = info.RequestType.Name.Replace("QueryHandler", "");
                var words = text.ToWords().ToList();

                if (words.Count > 3)
                {
                    if (words[0] == "Get" && words[1] == "By")
                        return string.Concat(words.Skip(3));
                }

                if (words[0] == "Get")
                    return string.Concat(words.Skip(1));
                return string.Concat(words);
            }
            throw new InvalidOperationException();
        }
        
        private string[] CommandVerbs = new string[] {"Add", "Update","Delete", "Remove", "Insert", "Create"};
    }
}