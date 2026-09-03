using System.Windows.Forms;
using AntdUI;
using WindowsFormsApp3.Forms.Main;
using WindowsFormsApp3.Models;
using Xunit;

namespace WindowsFormsApp3.Tests.Forms
{
    public class TemporaryImpositionParametersFormTests
    {
        [Fact]
        public void FlatSheetMode_ShouldInitializeAllInputsWithParent()
        {
            var parameters = new TemporaryImpositionParameters
            {
                PaperWidth = 320,
                PaperHeight = 232,
                MarginTop = 5,
                MarginBottom = 6,
                MarginLeft = 7,
                MarginRight = 8,
                IsPaperSizeUnlocked = true
            };

            using (var form = new TemporaryImpositionParametersForm(true, parameters))
            {
                Assert.NotNull(form);
                var inputs = GetAllInputs(form);
                // 纸张宽、纸张高、上边距、下边距、左边距、右边距
                Assert.Equal(6, inputs.Length);
                foreach (var input in inputs)
                {
                    Assert.NotNull(input.Parent);
                }
            }
        }

        [Fact]
        public void RollMaterialMode_ShouldInitializeAllInputsWithParent()
        {
            var parameters = new TemporaryImpositionParameters
            {
                FixedWidth = 320,
                MinLength = 150,
                MarginTop = 1,
                MarginBottom = 2,
                MarginLeft = 3,
                MarginRight = 4
            };

            using (var form = new TemporaryImpositionParametersForm(false, parameters))
            {
                Assert.NotNull(form);
                var inputs = GetAllInputs(form);
                // 固定宽度、最小长度、上边距、下边距、左边距、右边距
                Assert.Equal(6, inputs.Length);
                foreach (var input in inputs)
                {
                    Assert.NotNull(input.Parent);
                }
            }
        }

        private static AntdUI.Input[] GetAllInputs(Control control)
        {
            var list = new System.Collections.Generic.List<AntdUI.Input>();
            CollectInputs(control, list);
            return list.ToArray();
        }

        private static void CollectInputs(Control control, System.Collections.Generic.List<AntdUI.Input> list)
        {
            foreach (Control child in control.Controls)
            {
                if (child is AntdUI.Input input)
                {
                    list.Add(input);
                }
                CollectInputs(child, list);
            }
        }
    }
}
