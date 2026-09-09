using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// 短确认弹框的展示契约。调用方依据返回的 DialogResult 执行业务逻辑。
    /// </summary>
    public sealed class ModalRequest
    {
        public ModalRequest(Form owner, string title, string message, IReadOnlyList<ModalButtonSpec> buttons)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Title = title ?? string.Empty;
            Message = message ?? string.Empty;
            Buttons = buttons ?? throw new ArgumentNullException(nameof(buttons));
        }

        public Form Owner { get; }

        public string Title { get; }

        public string Message { get; }

        public IReadOnlyList<ModalButtonSpec> Buttons { get; }

        public AntdUI.TAMode ColorScheme { get; set; } = AntdUI.TAMode.Auto;

        public AntdUI.TType Icon { get; set; } = AntdUI.TType.None;

        public bool Keyboard { get; set; } = true;

        public bool MaskClosable { get; set; }
    }

    /// <summary>
    /// 弹框按钮的展示和结果契约。结果由调用方解释，渲染层不附加回调。
    /// </summary>
    public sealed class ModalButtonSpec
    {
        public ModalButtonSpec(string id, string text, DialogResult dialogResult, AntdUI.TTypeMini type)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("按钮 ID 不能为空。", nameof(id));
            }

            if (id == "OK" || id == "Cancel")
            {
                throw new ArgumentException("按钮 ID 不能使用 AntdUI 内置按钮名称。", nameof(id));
            }

            Id = id;
            Text = text ?? string.Empty;
            DialogResult = dialogResult;
            Type = type;
        }

        public string Id { get; }

        public string Text { get; }

        public DialogResult DialogResult { get; }

        public AntdUI.TTypeMini Type { get; }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }
    }
}
