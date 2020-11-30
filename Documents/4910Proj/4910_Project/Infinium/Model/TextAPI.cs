using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infinium.Model
{
    public class TextAPI
    {
        public static void SendMessage(string number, string message)
        {
            var accountSid = "ACfa2fe43bc85b07d8e120f252d1b5b8d5";
            var authToken = "a9ed39974c816447f82552d721948046";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber("+1" + number.Replace("-", "")));
            messageOptions.From = new PhoneNumber("+18644538990");
            messageOptions.Body = message;

            Console.WriteLine("sent to " + number);
            var msg = MessageResource.Create(messageOptions);
        }
    }
}