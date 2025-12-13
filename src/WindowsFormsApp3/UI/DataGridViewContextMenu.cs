using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace WindowsFormsApp3.UI
{
    /// <summary>
    /// DataGridView右键菜单组件，提供统一的右键菜单功能
    /// </summary>
    public class DataGridViewContextMenu
    {
        private DataGridView _dataGridView;
        private ContextMenuStrip _contextMenuStrip;
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
            _contextMenuStrip = new ContextMenuStrip();
            _dataGridView.ContextMenuStrip = _contextMenuStrip;

            // 监听单元格点击事件
            _dataGridView.CellMouseDown += DataGridView_CellMouseDown;
            _dataGridView.CellClick += DataGridView_CellClick;

            // 创建默认菜单项
            CreateDefaultMenuItems();
        }

        /// <summary>
        /// 创建默认菜单项
        /// </summary>
        private void CreateDefaultMenuItems()
        {
            _contextMenuStrip.Items.Clear();

            // 复制
            var copyItem = new ToolStripMenuItem("复制");
            copyItem.ShortcutKeys = Keys.Control | Keys.C;
            copyItem.Click += (sender, e) => CopySelectedCellValue();
            _contextMenuStrip.Items.Add(copyItem);

            // 剪切
            var cutItem = new ToolStripMenuItem("剪切");
            cutItem.ShortcutKeys = Keys.Control | Keys.X;
            cutItem.Click += (sender, e) => CutSelectedCellValue();
            _contextMenuStrip.Items.Add(cutItem);

            // 粘贴
            var pasteItem = new ToolStripMenuItem("粘贴");
            pasteItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteItem.Click += (sender, e) => PasteCellValue();
            _contextMenuStrip.Items.Add(pasteItem);

            // 删除
            var deleteItem = new ToolStripMenuItem("删除");
            deleteItem.Click += (sender, e) => DeleteSelectedCells();
            _contextMenuStrip.Items.Add(deleteItem);

            // 分隔线
            _contextMenuStrip.Items.Add(new ToolStripSeparator());

            // 刷新
            var refreshItem = new ToolStripMenuItem("刷新");
            refreshItem.Click += (sender, e) => RefreshDataGridView();
            _contextMenuStrip.Items.Add(refreshItem);

            // 添加自定义菜单项
            if (_customItems != null && _customItems.Count > 0)
            {
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                AddCustomMenuItems();
            }
        }

        /// <summary>
        /// 添加自定义菜单项
        /// </summary>
        private void AddCustomMenuItems()
        {
            foreach (var itemText in _customItems)
            {
                var customItem = new ToolStripMenuItem(itemText);
                customItem.Click += (sender, e) =>
                {
                    OnCustomMenuItemClick(new CustomMenuItemClickEventArgs(itemText, CurrentColumnName));
                };
                _contextMenuStrip.Items.Add(customItem);
            }
        }

        /// <summary>
        /// 刷新右键菜单
        /// </summary>
        public void RefreshContextMenu()
        {
            CreateDefaultMenuItems();
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