using System;
using System.Windows.Forms;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 将会修改窗体集合的操作延后到当前控件回调完成后执行。
    /// </summary>
    internal static class UiActionDispatcher
    {
        internal static void Defer(Control control, Action action)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (control.IsDisposed || control.Disposing)
            {
                return;
            }

            control.BeginInvoke(action);
        }
    }
}
