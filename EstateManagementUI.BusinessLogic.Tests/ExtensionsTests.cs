using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateManagementUI.BusinessLogic.Common;
using Shared.Exceptions;
using Shouldly;

namespace EstateManagementUI.BusinessLogic.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void ExceptionHelper_GetCombinedExceptionMessages_ExpectedMessageReturned() {
            Exception innerInnerException = new Exception("This is inner inner exception");
            Exception innerException = new Exception("This is inner exception",innerInnerException);
            Exception exception = new Exception("This is exception", innerException);

            String messages = exception.GetCombinedExceptionMessages();
            messages.ShouldBe($"This is exception{Environment.NewLine}This is inner exception{Environment.NewLine}This is inner inner exception{Environment.NewLine}");
        }

        [Fact]
        public void ExceptionHelper_GetCombinedExceptionMessages_ExceptionIsNull_NothingReturned()
        {
            Exception exception = null;

            String messages = ExceptionHelper.GetCombinedExceptionMessages(exception);
            messages.ShouldBeNullOrEmpty();
        }
    }
}
