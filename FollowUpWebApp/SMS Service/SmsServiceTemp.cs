using System.IO;
using System.Net;

namespace FollowUpWebApp.SMS_Service
{
    public class SmsServiceTemp
    {
        private ConfigService _config;
        //private Cache _cache;

        //private string sessionId_cahe_key = "SmsSessionId_" + "GetSessionId";

        public SmsServiceTemp()
        {
            _config = new ConfigService();
            //_cache = HttpContext.Current.Cache;
        }

        //Default method for making request to the SMS gateway. This method is not likely to be changed no matter what 
        //SMS gateway provider you want to use in the future.
        private string MakeHttpRequest(string url)
        {
            //Initialize the web request
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.ContentLength = 0;

            webReq.Method = "POST";//We're making a post request. This is the recommended method by the gateway.
            webReq.Timeout = 600000;//Set the timeout for the request

            var webResp = (HttpWebResponse)webReq.GetResponse();

            //Read the response and output it.
            Stream answer = webResp.GetResponseStream();
            StreamReader _answer = new StreamReader(answer);

            string result = _answer.ReadToEnd();

            return result;
        }


        //Process the response from the sms gateway. By default, the gateway's response is in format below
        //OK: [RESPONSE-Message] -or- ERR: [ERROR NUMBER]: [ERROR DESCRIPTION]
        public string GetResponseMessage(string response, out bool success, out string errMsg)
        {
            //if the response contains 'OK', then the request was successful
            //bool isSuccess = response.Substring(0, response.IndexOf(":") + 1).Contains("OK");
            bool isSuccess = false;
            string errDesc = null;
            string code = null;
            string myresponse = response;

            if (myresponse.ToString().Contains("OK"))
            {
                isSuccess = true;
            }
            else if (myresponse.ToString().Contains("2907"))
            {
                errDesc = "Messaging Service not available";
                code = "FAIL";
            }
            else if (myresponse.ToString().Contains("2906"))
            {
                errDesc = "Insufficient Credit for messaging";
                code = "FAIL";
            }
            else
            {
                errDesc = "Error Sending Message";
                code = "FAIL";
            }

            success = isSuccess;
            errMsg = errDesc;
            return code;
        }

        public string Send(Sms sms)
        {
            //string sessionId = GetSessionId(); //Get the session id
            string smsUrl = _config.SmsUrl; //Get the sms gateway url from the config file

            //Form the command for sending message. You can download the API documentation for full list of commands
            //from http://kudisms.net
            string smsCmd = $"username={_config.SmsAccount}&password={_config.SubAccountPwd}&sender={sms.Sender}" +
                            $"&recipient={sms.Recipient}&message={sms.Message}";

            bool isSuccess = false;
            string errMsg = null;
            //Send sms message
            string response = MakeHttpRequest(smsUrl + smsCmd);

            //Process the response from the gateway
            string code = GetResponseMessage(response, out isSuccess, out errMsg);

            ////401 error code indicate invalid Session ID. If the session id is not valid, then delete it from cache and make a 
            ////request to get a new session id from the sms gateway
            //if (code == "401")
            //{
            //    _cache.Remove(sessionId_cahe_key);//delete the session id from the cache

            //    sessionId = GetSessionId(); //Get the session id
            //    smsCmd = String.Format("?cmd=sendmsg&sessionid={0}&message={1}&sender={2}" +
            //                              "&sendto={3}&msgtype=0", sessionId, sms.Message, sms.SenderId, sms.Numbers);

            //    return makeHttpRequest(smsUrl + smsCmd);//resend the sms to the gateway
            //}

            return response;
        }
    }
}