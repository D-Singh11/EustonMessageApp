using System;
using System.Windows.Controls;
using BusinessLayer;
using MessageApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMessageApp
{
    [TestClass]
    public class MessageTest
    {
        Message target = new Message();


        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Message id must start with a letter.")]
        public void messageMustStartWithLetter()
        {
            TextBox tb = new TextBox();
            target.pMessageId = "99999999";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Message id must not have letters at other than first position")]
        public void onlyFirstLetterOFMessageIdShouldBeAlphabet()
        {
            target.pMessageId = "999m99999";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Message id must start with a s,m or t letter.")]
        public void firstLetterOFMessageIdCanOnlyBe_S_or_M_or_T()
        {
            target.pMessageId = "a123456789";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Message id must be of excatly 10 letters.")]
        public void messageIdMustBeOfExactlyf10CharacterLong()
        {
            target.pMessageId = "s12345678";
        }

        [TestMethod]
        public void messageAssignCorrectIdToProperty()
        {
            string expected = "s123456789";
            target.pMessageId = "s123456789";
            Assert.AreEqual(expected, target.pMessageId);
        }
    }
}