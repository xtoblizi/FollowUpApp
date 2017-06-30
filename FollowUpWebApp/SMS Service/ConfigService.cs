using System;
using System.Configuration;

namespace FollowUpWebApp.SMS_Service
{
    public class ConfigService
    {
        /// <summary>
        /// Get the SMS gateway url
        /// </summary>
        public string SmsUrl => GetAppSetting(typeof(string), "SmsUrl").ToString();

        /// <summary>
        /// Get the gateway account username to use in sending sms message
        /// </summary>
        public string SmsAccount => GetAppSetting(typeof(string), "SmsUserName").ToString();



        /// <summary>
        /// Get the sub account password for the sms gateway
        /// </summary>
        public string SubAccountPwd => GetAppSetting(typeof(string), "SubAccountPass").ToString();

        private static object GetAppSetting(Type expectedType, string key)
        {
            string value = ConfigurationManager.AppSettings[key]; //.Get(key);

            if (value == null)
            {
                throw new Exception(
                    $"The config file does not have the key '{key}' defined in the AppSetting section.");
            }

            if (expectedType == typeof(int))
            {
                return int.Parse(value);
            }

            if (expectedType == typeof(string))
            {
                return value;
            }
            else
                throw new Exception("Type not supported.");
        }
    }
}