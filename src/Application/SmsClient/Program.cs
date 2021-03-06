// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-17  4:31 PM
// ***********************************************************************
// <copyright file="Program.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmsClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintHelp();
            string command;
            do
            {
                Console.WriteLine("Command:");
                command = Console.ReadLine();
                if (command == null)
                {
                    command = "";
                    continue;
                }

                List<string> parameters = command.Split(' ').ToList();

                int index = parameters.IndexOf("-C");
                if (index == -1 || parameters.Count < index + 2)
                {
                    Console.WriteLine("Missing cellphone numbers.");
                    continue;
                }

                string cellphones = parameters[index + 1];
                if (string.IsNullOrEmpty(cellphones))
                {
                    Console.WriteLine("Missing cellphone numbers.");
                    continue;
                }

                string message;
                int messageIndex = parameters.IndexOf("-M");
                if (messageIndex == -1 || parameters.Count < messageIndex + 2)
                {
                    message = "验证码：123456";
                }
                else
                {
                    message = parameters[messageIndex + 1];
                }

                string signature;
                int signatureIndex = parameters.IndexOf("-S");
                if (signatureIndex == -1 || parameters.Count < signatureIndex + 2)
                {
                    signature = "金银猫";
                }
                else
                {
                    signature = parameters[signatureIndex + 1];
                }

                SendMessageAsync(cellphones, message, signature).Wait();
            } while (command.ToUpperInvariant() != "QUIT");
        }

        private static void PrintHelp()
        {
            string helpText = "-C[REQUIRED]     CellphoneNumbers: split with ',', eg. 15812341234,15912341234\n" +
                              "-M[OPTIONAL]     Message: a string represents the content of the sms, eg. TestMessage\n" +
                              "-S[OPTIONAL]     Signature: a string represents the sender of the sms, eg. JinYinMao";
            Console.WriteLine(helpText);
        }

        private static async Task SendMessageAsync(string cellphones, string message, string signature)
        {
            Console.WriteLine("Calling the back-end API");

            string smsServiceAddress = ConfigurationManager.AppSettings.Get("SmsServiceAddress");
            string apiBaseAddress = string.IsNullOrEmpty(smsServiceAddress) ? "https://jym-web-dev-sms.jinyinmao.com.cn/" : smsServiceAddress;

            ApiKeyAuthDelegatingHandler delegatingHandler = new ApiKeyAuthDelegatingHandler();

            HttpClient client = HttpClientFactory.Create(delegatingHandler);

            SmsMessage smsMessage = new SmsMessage { Cellphones = cellphones, Channel = "100001", Message = message, Signature = signature };

            HttpResponseMessage response = await client.PostAsJsonAsync(apiBaseAddress + "Send", smsMessage);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            }
            else
            {
                Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}