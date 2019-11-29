using System;
using BusinessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMessageApp
{
    [TestClass]
    public class SmsTest
    {
        SMS target = new SMS();


        [TestMethod]
        [ExpectedException(typeof(Exception),"SMS sender number must start with + symbol")]
        public void senderNumberMustStartWithPlusSymbol()
        {
            string sender = "9999999999";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "+ symbol in sender's number must only be at first position and not in between.")]
        public void senderNumberPlusSymbolMustBeFirstCharacter()
        {
            string sender = "99999+99999";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "sender's phone number must be in all digits.")]
        public void senderNumberBeAllDigits()
        {
            string sender = "+aaaaaaaaa";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Sender's number must be minimum of 9 characters.")]
        public void senderNumberBeMinimumOf9Charcters()
        {
            string sender = "+123";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Sender's number must not exceed more than of 15 characters.")]
        public void senderNumberBeMaximumOf15Charcters()
        {
            string sender = "+1234567890123456789";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),"Sender's number must not contain special characters.")]
        public void senderNumberMustNotHaveSpecialCharcters()
        {
            string sender = "+1234$56=789";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "sender must only have one phone number.")]
        public void senderMustOnlyHaveOneNumber()
        {
            string sender = "+123456789 +123456789";
            target.pSender = sender;
        }

        [TestMethod]
        public void validNumberMustBeAssignedToProperty()
        {
            string expected = "+123456789";
            target.pSender = "+123456789";

            Assert.AreEqual(expected, target.pSender);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "SMS text must not be blank.")]
        public void emptyTextMessageMustThroughException()
        {
            target.pMessageText = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "SMS text must not be more than 140 characters.")]
        public void messageTextOver140CharactersMustThroughException()
        {
            target.pMessageText = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        }

        [TestMethod]
        public void validLengthTextMustBeAssignedToProperty()
        {
            string expected = "aaaaaaaaaaaaaaaaaaaaaaa";
            target.pMessageText = "aaaaaaaaaaaaaaaaaaaaaaa";
            Assert.AreEqual(expected, target.pMessageText);

        }
    }
}
