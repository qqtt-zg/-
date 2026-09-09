using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// DataGridView右键菜单组件，提供统一的右键菜单功能
    /// </summary>
    public class DataGridViewContextMenu
    {
        private DataGridView _dataGridView;
        private List<string> _customItems = new List<string>();

        /// <summary>
        /// 获取或设置当前点击的列名
        /// </summary>
        public string CurrentColumnName { get; private set; }

        /// <summary>
        /// 获取或设置自定义菜单项
        /// </summary>
        public List<string> CustomItems
        {
            get { return _customItems; }
            set 
            { 
                _customItems = value ?? new List<string>();
                RefreshContextMenu();
            }
        }

        /// <summary>
        /// 自定义菜单项点击事件
        /// </summary>
        public event EventHandler<CustomMenuItemClickEventArgs> CustomMenuItemClick;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataGridView">要绑定右键菜单的DataGridView控件</param>
        public DataGridViewContextMenu(DataGridView dataGridView)
        {
            if (dataGridView == null)
                throw new ArgumentNullException(nameof(dataGridView));

            _dataGridView = dataGridView;
            Initialize();
        }

        /// <summary>
        /// 初始化右键菜单
        /// </summary>
        private void Initialize()
        {
            // 监听单元格点击事件
            _dataGridView.CellMouseDown += DataGridView_CellMouseDown;
            _dataGridView.CellMouseUp += DataGridView_CellMouseUp;
            _dataGridView.CellClick += DataGridView_CellClick;
            _dataGridView.KeyDown += DataGridView_KeyDown;
        }

        /// <summary>
        /// 创建默认菜单项
        /// </summary>
        private IReadOnlyList<ContextMenuItemSpec> CreateDefaultMenuItems()
        {
            var items = new List<ContextMenuItemSpec>
            {
                new ContextMenuItemSpec("copy", "复制")
                {
                    ShortcutText = "Ctrl+C"
                },
                new ContextMenuItemSpec("cut", "剪切")
                {
                    ShortcutText = "Ctrl+X"
                },
                new ContextMenuItemSpec("paste", "粘贴")
                {
                    ShortcutText = "Ctrl+V"
                },
                new ContextMenuItemSpec("delete", "删除")
                {
                    IsDangerous = true
                },
                ContextMenuItemSpec.Divider(),
                new ContextMenuItemSpec("refresh", "刷新")
            };

            if (_customItems != null && _customItems.Count > 0)
            {
                items.Add(ContextMenuItemSpec.Divider());
                AddCustomMenuItems(items);
            }

            return items;
        }

        /// <summary>
        /// 添加自定义菜单项
        /// </summary>
        private void AddCustomMenuItems(ICollection<ContextMenuItemSpec> items)
        {
            for (var index = 0; index < _customItems.Count; index++)
            {
                var itemText = _customItems[index];
                items.Add(new ContextMenuItemSpec($"custom-{index}", itemText)
                {
                    Tag = itemText
                });
            }
        }

        /// <summary>
        /// 刷新右键菜单
        /// </summary>
        public void RefreshContextMenu()
        {
            // 菜单在显示时按当前配置创建，无需保留原生菜单实例。
        }

        /// <summary>
        /// 单元格点击事件处理
        /// </summary>
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateCurrentColumnName(e);
        }

        /// <summary>
        /// 单元格鼠标按下事件处理
        /// </summary>
        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                UpdateCurrentColumnName(new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
            }
        }

        /// <summary>
        /// 右键释放时按原生菜单的目标位置显示 AntdUI 菜单。
        /// </summary>
        private void DataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            AntdUiContextMenuRenderer.Show(new ContextMenuRequest(
                _dataGridView,
                CreateDefaultMenuItems())
            {
                Location = new Point(e.X, e.Y),
                UseMousePosition = true
            }, HandleMenuItemSelected);
        }

        /// <summary>
        /// 保留原生菜单中 Ctrl+C、Ctrl+X 和 Ctrl+V 的快捷键语义。
        /// </summary>
        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (ContextMenuRequest.IsKeyboardInvocation(e.KeyCode, e.Modifiers))
            {
                AntdUiContextMenuRenderer.Show(new ContextMenuRequest(
                    _dataGridView,
                    CreateDefaultMenuItems())
                {
                    Location = ContextMenuRequest.GetKeyboardInvocationLocation(_dataGridView)
                }, HandleMenuItemSelected);

                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.Modifiers != Keys.Control)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.C:
                    CopySelectedCellValue();
                    break;
                case Keys.X:
                    CutSelectedCellValue();
                    break;
                case Keys.V:
                    PasteCellValue();
                    break;
                default:
                    return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// 保持菜单命令与原有业务处理方法一一对应。
        /// </summary>
        private void HandleMenuItemSelected(ContextMenuItemSpec item)
        {
            switch (item.Id)
            {
                case "copy":
                    CopySelectedCellValue();
                    break;
                case "cut":
                    CutSelectedCellValue();
                    break;
                case "paste":
                    PasteCellValue();
                    break;
                case "delete":
                    DeleteSelectedCells();
                    break;
                case "refresh":
                    RefreshDataGridView();
                    break;
                default:
                    if (item.Id.StartsWith("custom-", StringComparison.Ordinal))
                    {
                        OnCustomMenuItemClick(new CustomMenuItemClickEventArgs((string)item.Tag, CurrentColumnName));
                    }
                    break;
            }
        }

        /// <summary>
        /// 更新当前点击的列名
        /// </summary>
        private void UpdateCurrentColumnName(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                CurrentColumnName = _dataGridView.Columns[e.ColumnIndex].Name;
            }
        }

        /// <summary>
        /// 复制选中单元格的值
        /// </summary>
        private void CopySelectedCellValue()
        {
            if (_dataGridView.CurrentCell != null)
            {
                Clipboard.SetText(_dataGridView.CurrentCell.Value?.ToString() ?? string.Empty);
            }
        }

        /// <summary>
        /// 剪切选中单元格的值
        /// </summary>
        private void CutSelectedCellValue()
        {
            if (_dataGridView.CurrentCell != null && _dataGridView.CurrentCell.ReadOnly == false)
            {
                Clipboard.SetText(_dataGridView.CurrentCell.Value?.ToString() ?? string.Empty);
                _dataGridView.CurrentCell.Value = string.Empty;
            }
        }

        /// <summary>
        /// 粘贴值到选中单元格
        /// </summary>
        private void PasteCellValue()
        {
            if (_dataGridView.CurrentCell != null && _dataGridView.CurrentCell.ReadOnly == false && Clipboard.ContainsText())
            {
                _dataGridView.CurrentCell.Value = Clipboard.GetText();
            }
        }

        /// <summary>
        /// 删除选中的单元格内容
        /// </summary>
        private void DeleteSelectedCells()
        {
            foreach (DataGridViewCell cell in _dataGridView.SelectedCells)
            {
                if (!cell.ReadOnly)
                {
                    cell.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        private void RefreshDataGridView()
        {
            _dataGridView.Refresh();
        }

        /// <summary>
        /// 触发自定义菜单项点击事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnCustomMenuItemClick(CustomMenuItemClickEventArgs e)
        {
            CustomMenuItemClick?.Invoke(this, e);
        }

        /// <summary>
        /// 自定义菜单项点击事件参数
        /// </summary>
        public class CustomMenuItemClickEventArgs : EventArgs
        {
            /// <summary>
            /// 菜单项文本
            /// </summary>
            public string MenuItemText { get; }

            /// <summary>
            /// 列名
            /// </summary>
            public string ColumnName { get; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="menuItemText">菜单项文本</param>
            /// <param name="columnName">列名</param>
            public CustomMenuItemClickEventArgs(string menuItemText, string columnName)
            {
                MenuItemText = menuItemText;
                ColumnName = columnName;
            }
        }
    }
}
