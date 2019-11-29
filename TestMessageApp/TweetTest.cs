using System;
using BusinessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMessageApp
{
    [TestClass]
    public class TweetTest
    {
        Tweet target = new Tweet();


        [TestMethod]
        [ExpectedException(typeof(Exception), "Twitter handle must start with @ symbol.")]
        public void senderTwitterHandleMustStartWithAtSymbol()
        {
            string sender = "abc";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Twitter handle must not have @ symbol other than first position.")]
        public void senderTwitterHandleAtSymbolMustBeFirstCharacter()
        {
            string sender = "aaa@aaa";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Twitter handle must be minimum of 1 character excluding @ symbol")]
        public void senderTwitterHandleMustBeMinimumOf1CharctersExcludingAtSymbol()
        {
            string sender = "";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Twitter handle must not be more than 15 characters")]
        public void senderTwitterHandleMustBeMaximumOf15CharctersExcludingAtSymbol()
        {
            string sender = "@abcdefghijklmnopqrst";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Twitter handle must not contain special symbols")]
        public void senderTwitterHandleMustNotHaveSpecialCharctersExceptUnderscore()
        {
            string sender = "@abcdef$$gg";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Twitter handle must not contain two @ symbols")]
        public void senderTwitterHandleMustOnlyHaveOneAtSymbol()
        {
            string sender = "@@abc";
            target.pSender = sender;
        }

        [TestMethod]
        public void senderTwitterHandleMustBeAssignedToProperty()
        {
            string expected = "@abcdef";
            target.pSender = "@abcdef";

            Assert.AreEqual(expected, target.pSender);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Tweet text must not be blank.")]
        public void emptyTextMessageMustThroughException()
        {
            target.pMessageText = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Tweet text must not be more than 140 charactors.")]
        public void messageTextOver140CharactersMustThroughException()
        {
            target.pMessageText = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        }

        [TestMethod]
        public void validLengthTextMustBeAssignedToProperty()
        {
            string expected = "Hello this is tweet";
            target.pMessageText = "Hello this is tweet";
            Assert.AreEqual(expected, target.pMessageText);

        }
    }
}
