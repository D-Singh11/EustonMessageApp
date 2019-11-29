using System;
using System.Collections.Generic;
using System.Windows.Controls;
using BusinessLayer;
using MessageApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMessageApp
{
    [TestClass]
    public class MessageOperationsTest
    {
        MessageOperationsFacade target = new MessageOperationsFacade();
        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid SMS sender input, system must not process message")]
        public void invalidSmsSender()
        {
            List<string> sms = new List<string>();
            string messageId = "s123456789";
            sms.Add("sender: 99999");              // invalid SMS sender
            sms.Add("Text: Hello");
            target.buildSmsMessage(sms, messageId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid SMS text input, system must not process message")]
        public void invalidSmsText()
        {
            List<string> sms = new List<string>();
            string messageId = "s123456789";
            sms.Add("sender: +9999999999");
            sms.Add("");                // invalid SMS text
            target.buildSmsMessage(sms, messageId);
        }

        [TestMethod]
        public void validSmsInputShouldProcessMessage()
        {
            List<string> sms = new List<string>();
            string messageId = "s123456789";
            sms.Add("sender: +99999999999");
            sms.Add("Text: Hello All asap");
            SMS actual = target.buildSmsMessage(sms, messageId);

            Assert.AreEqual(actual.pMessageId, messageId);
            Assert.AreEqual(actual.pSender, "+99999999999");
            Assert.AreEqual(actual.pMessageText, "hello all ASAP <As soon as possible>");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid tweet sender input, system must not process message")]
        public void invalidTweetSender()
        {
            List<string> tweet = new List<string>();
            string messageId = "t123456789";
            tweet.Add("sender: jhj@");          // invalid tweet sender
            tweet.Add("Text: Hello");
            target.buildTwitterMessage(tweet, messageId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid tweet text input, system must not process message")]
        public void invalidTweetText()
        {
            List<string> tweet = new List<string>();
            string messageId = "t123456789";
            tweet.Add("sender: +9999999999");
            tweet.Add("");                          // invalid tweet text
            target.buildTwitterMessage(tweet, messageId);
        }

        [TestMethod]
        public void validTweetInputShouldProcessMessage()
        {
            List<string> tweet = new List<string>();
            string messageId = "t123456789";
            tweet.Add("sender: @abcc");
            tweet.Add("Text: Hello All asap");
            Tweet actual = target.buildTwitterMessage(tweet, messageId);

            Assert.AreEqual(actual.pMessageId, messageId);
            Assert.AreEqual(actual.pSender, "@abcc");
            Assert.AreEqual(actual.pMessageText, "hello all ASAP <As soon as possible>");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid email sender input, system must not process message")]
        public void invalidEmailSender()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: jhj@");              // invalid sender's email
            email.Add("subject: Urgent");
            email.Add("Text: Hello");
            target.buildEmailMessage(email, messageId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid standard email's subject input, system must not process message")]
        public void invalidStandardEmalSubject()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: ");             // invalid subject
            email.Add("Text: Hello");
            target.buildEmailMessage(email, messageId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid standard email's text, system must not process message")]
        public void invalidEmalText()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: Urgent");
            email.Add("Text:");                         // invalid text
            target.buildEmailMessage(email, messageId);
        }

        [TestMethod]
        public void validStandardEmailInputShouldProcessMessage()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: Urgent");
            email.Add("Text: Hello All asap");
            Email actual = target.buildEmailMessage(email, messageId);

            Assert.AreEqual(actual.pMessageId, messageId);
            Assert.AreEqual(actual.pSender, "abc@bb.com");
            Assert.AreEqual(actual.pMessageText, "hello all asap");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid SIR email's subject input, system must not process message")]
        public void invalidSirEmalSubject()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: SIR 999/333/333");          // invalid subject
            email.Add("sport centre code: 00-000-00");
            email.Add("Text: Hello");
            target.buildEmailMessage(email, messageId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid SIR email's sport centre code, system must not process message")]
        public void invalidSirEmalSportCentreCode()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: SIR 11/11/1111");
            email.Add("sport centre code: 9999999");    // invalid sport centre code
            email.Add("Text: Hello all");
            target.buildEmailMessage(email, messageId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "If invalid SIR email's nature of incident, system must not process message")]
        public void invalidSirEmalIncidentNature()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: SIR 11/11/1111");
            email.Add("sport centre code: 00-000-00");
            email.Add("nature of incident: Raider");        // invalid nature of incident
            email.Add("Text: Hello all");
            target.buildEmailMessage(email, messageId);
        }

        [TestMethod]
        public void validSirEmailInputShouldProcessMessage()
        {
            List<string> email = new List<string>();
            string messageId = "m123456789";
            email.Add("sender: abc@bb.com");
            email.Add("subject: 11/11/1111");
            email.Add("sport centre code: 00-000-00");
            email.Add("nature of incident: Raid");        // invalid nature of incident
            email.Add("Text: Hello all");
            target.buildEmailMessage(email, messageId);
        }
    }
}