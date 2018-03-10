/*****************************************
*           Litecoin Interface           *
*               by Szenmu                *
******************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Szenmu.Litecoin.Model;

namespace Szenmu.Litecoin
{
    public class Litecoin
    {
        private string Username { get; set; }
        private string Password { get; set; }
        private string RPCHost { get; set; }
        private string RPCPort { get; set; }

        public Litecoin(string username, string password, string rpchost, string rpcport)
        {
            Username = username;
            Password = password;
            RPCHost = rpchost;
            RPCPort = rpcport;
        }
    
        private string MakeRequest(Command command)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create($"http://{RPCHost}:{RPCHost}");
            webRequest.Credentials = new NetworkCredential(Username,Password);
            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";

            string s = JsonConvert.SerializeObject(command.Cmd);
            byte[] byteArray = Encoding.UTF8.GetBytes(s);
            webRequest.ContentLength = byteArray.Length;
            Stream dataStream = webRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                WebResponse webResponse = webRequest.GetResponse();
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    return reader.ReadToEnd();
                }
            }

        }
        public void GetAccountAddress(string account)
        {
            /*
	        "params":{
                "account": ACCOUNT_NAME
            }
            */
            Dictionary<string, string> paramaters = new Dictionary<string, string>();
            paramaters.Add("account", account);

            Console.WriteLine($"Getting address for user: {account}");

            Command command = new Command("getaccountaddress", paramaters);
            string responseString = MakeRequest(command);

            ResponseGetAccountAddress response = JsonConvert.DeserializeObject<ResponseGetAccountAddress>(responseString);

            Console.WriteLine(response.Result);
            Console.ReadLine();
        }
        public void GetBalanceByAccount(string account)
        {
            /*
	        "params":{
                "account": ACCOUNT_NAME
            }
            */

            Dictionary<string, string> paramaters = new Dictionary<string, string>();
            paramaters.Add("account", account);

            Console.WriteLine("");
            Console.WriteLine($"Getting balance for user: {account}");

            Command command = new Command("getbalance", paramaters);
            string responseString = MakeRequest(command);

            ResponseGetBalanceByAccount response = JsonConvert.DeserializeObject<ResponseGetBalanceByAccount>(responseString);

            Console.WriteLine(response.Result);
            Console.ReadLine();
        }
        public void SendFromAccount(string account, string address, decimal amount)
        {

            Dictionary<string, string> paramaters = new Dictionary<string, string>();
            paramaters.Add("fromaccount", account);
            paramaters.Add("toaddress", address);
            paramaters.Add("amount", amount.ToString());

            Command command = new Command("sendfrom", paramaters);
            string responseString = MakeRequest(command);

            ResponseSendFromAccount response = JsonConvert.DeserializeObject<ResponseSendFromAccount>(responseString);
            if (response.Error != null)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine($"Error code: {response.Error.Code}");
                Console.WriteLine($"Error message: {response.Error.Message}");
            }
            Console.WriteLine(response.Result);
            Console.ReadLine();
        }

    }
}
