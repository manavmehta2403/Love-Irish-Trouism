using LIT.Modules.ModuleName.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIT.Modules.ModuleName.Tests.ViewModels
{
    public class ViewAViewModelFixture
    {
        dynamic _messageServiceMock;
        const string MessageServiceDefaultMessage = "Some Value";

        public ViewAViewModelFixture()
        {
            var messageService = "Some value";
            _messageServiceMock = MessageServiceDefaultMessage;
        }

        [TestMethod]
        public void MessagePropertyValueUpdated()
        {

            _messageServiceMock = MessageServiceDefaultMessage;

            Assert.AreEqual(MessageServiceDefaultMessage, _messageServiceMock);
        }
    }
}
