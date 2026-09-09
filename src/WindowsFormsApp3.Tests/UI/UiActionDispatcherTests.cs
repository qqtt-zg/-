using System;
using System.Windows.Forms;
using WindowsFormsApp3.UI;
using Xunit;

namespace WindowsFormsApp3.Tests.UI
{
    public class UiActionDispatcherTests
    {
        [Fact]
        public void Defer_Runs_After_The_Current_UI_Callback_Returns()
        {
            using var form = new Form();
            _ = form.Handle;
            var invoked = false;

            UiActionDispatcher.Defer(form, () => invoked = true);

            Assert.False(invoked);
            Application.DoEvents();
            Assert.True(invoked);
        }
    }
}
