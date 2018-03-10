using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Szenmu.Litecoin.Model
{
    class Command
    {
        public JObject Cmd { get; set; }

        public Command(string method, Dictionary<string,string> paramList)
        {
            Cmd = new JObject();
            Cmd.Add(new JProperty("id", "1"));
            Cmd.Add(new JProperty("method", method));

            if (paramList.Keys.Count == 0)
            {
                Cmd.Add(new JProperty("params", new JObject()));
            }
            else
            {
                JObject props = new JObject();
                foreach (KeyValuePair<string, string> param in paramList)
                {
                    props.Add(new JProperty(param.Key, param.Value));
                }
                Cmd.Add(new JProperty("params", props));
            }
        }
    }
}
