using System;
using BusinessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMessageApp
{
    [TestClass]
    public class EmailTest
    {
        Email target = new Email();


        [TestMethod]
        [ExpectedException(typeof(Exception), "Sender' email address must not be blank")]
        public void senderEmailMustNotBeBlank()
        {
            string sender = "";
            target.pSender = sender;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Sender' email address must be in abc@abc.com format")]
        public void senderEmailMustBeInCorrectFormat()
        {
            string sender = "abc.com";
            target.pSender = sender;
        }

        [TestMethod]
        public void validEmailMustBeAssignedToProperty()
        {
            string expected = "abc@abc.com";
            target.pSender = "abc@abc.com";

            Assert.AreEqual(expected, target.pSender);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email text must not be blank.")]
        public void emptyTextMessageMustThroughException()
        {
            target.pMessageText = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email text must not be more than 1028 characters.")]
        public void messageTextOver1028CharactersMustThroughException()
        {
            target.pMessageText = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        }

        [TestMethod]
        public void validLengthTextMustBeAssignedToProperty()
        {
            string expected = "aaaaaaaaaaaaaaaaaaaaaaa";
            target.pMessageText = "aaaaaaaaaaaaaaaaaaaaaaa";
            Assert.AreEqual(expected, target.pMessageText);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email subject must not be blank")]
        public void emptySubjectMustThroughException()
        {
            target.pSubject = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email subject must not be more than 20 characters.")]
        public void subjectMustBeMaximumoOf20Characters()
        {
            target.pSubject = "aaaaaaaaaaaaaaaaaaaaaaaaaaa";
        }

        [TestMethod]
        public void validSubjectMustBeAssignedToProperty()
        {
            string expected = "Subject";
            target.pSubject = "Subject";
            Assert.AreEqual(expected, target.pSubject);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email's SIR subject must be in SIR 11/11/2000 format.")]
        public void invalidSirSubjectMustThroughException()
        {
            target.pSubject = "SIR subject";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email must have SIR subject only once.")]
        public void sirSubjectMustOnlyHaveOneInstance()
        {
            target.pSubject = "SIR 11/11/2000 SIR 11/11/2000";
        }

        [TestMethod]
        public void validSirSubjectMustBeAssignedToProperty()
        {
            string expected = "SIR 11/11/2000";
            target.pSubject = "SIR 11/11/2000";
            Assert.AreEqual(expected, target.pSubject);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email's SIR subject must not be blank.")]
        public void emptySirCentreCodeMustThroughException()
        {
            target.pCentreCode = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email's sport centre code must be in 200-000-00 format.")]
        public void sirCentreCodeMustBeInCorrectFormat()
        {
            target.pCentreCode = "222-222-222";
        }

        [TestMethod]
        public void validSirCentreCodeMustBeAssignedToProperty()
        {
            string expected = "00-000-00";
            target.pCentreCode = "00-000-00";
            Assert.AreEqual(expected, target.pCentreCode);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email's nature of incident must not be blank.")]
        public void emptySirIncidentNatureMustThroughException()
        {
            target.pIncidentNature = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Email's nature of incident must be in correct format.")]
        public void sirIncidentNatureMustBeInCorrectFormat()
        {
            target.pIncidentNature = "AAAAAAAAAAAAAAAAAAAAA";
        }

        [TestMethod]
        public void validSirIncidentNatureMustBeAssignedToProperty()
        {
            string expected = "Raid";
            target.pIncidentNature = "Raid";
            Assert.AreEqual(expected, target.pIncidentNature);
        }


    }
}
