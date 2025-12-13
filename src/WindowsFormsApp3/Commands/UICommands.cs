using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsFormsApp3.Commands
{
    /// <summary>
    /// 编辑单元格命令
    /// </summary>
    public class EditCellCommand : CommandBase
    {
        private readonly DataGridView _dataGridView;
        private readonly int _rowIndex;
        private readonly int _columnIndex;
        private readonly object _oldValue;
        private readonly object _newValue;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataGridView">数据网格视图</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public EditCellCommand(DataGridView dataGridView, int rowIndex, int columnIndex, object oldValue, object newValue)
            : base($"编辑单元格: [{rowIndex},{columnIndex}]")
        {
            _dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// 执行编辑操作
        /// </summary>
        protected override void OnExecute()
        {
            if (_rowIndex >= 0 && _rowIndex < _dataGridView.Rows.Count &&
                _columnIndex >= 0 && _columnIndex < _dataGridView.Columns.Count)
            {
                _dataGridView.Rows[_rowIndex].Cells[_columnIndex].Value = _newValue;
            }
        }

        /// <summary>
        /// 撤销编辑操作
        /// </summary>
        protected override void OnUndo()
        {
            if (_rowIndex >= 0 && _rowIndex < _dataGridView.Rows.Count &&
                _columnIndex >= 0 && _columnIndex < _dataGridView.Columns.Count)
            {
                _dataGridView.Rows[_rowIndex].Cells[_columnIndex].Value = _oldValue;
            }
        }
    }

    /// <summary>
    /// 排序命令
    /// </summary>
    public class SortCommand : CommandBase
    {
        private readonly DataGridView _dataGridView;
        private readonly int _columnIndex;
        private readonly SortOrder _direction;
        private readonly DataGridViewRow[] _originalOrder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataGridView">数据网格视图</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="direction">排序方向</param>
        public SortCommand(DataGridView dataGridView, int columnIndex, SortOrder direction)
            : base($"排序列: {dataGridView.Columns[columnIndex].Name}")
        {
            _dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            _columnIndex = columnIndex;
            _direction = direction;

            // 保存原始顺序
            _originalOrder = new DataGridViewRow[_dataGridView.Rows.Count];
            _dataGridView.Rows.CopyTo(_originalOrder, 0);
        }

        /// <summary>
        /// 执行排序操作
        /// </summary>
        protected override void OnExecute()
        {
            // 简化实现：仅按字符串排序
            var rows = new DataGridViewRow[_dataGridView.Rows.Count];
            _dataGridView.Rows.CopyTo(rows, 0);

            if (_direction == SortOrder.Ascending)
            {
                Array.Sort(rows, (r1, r2) => string.Compare(r1.Cells[_columnIndex].Value?.ToString() ?? "", r2.Cells[_columnIndex].Value?.ToString() ?? ""));
            }
            else
            {
                Array.Sort(rows, (r1, r2) => string.Compare(r2.Cells[_columnIndex].Value?.ToString() ?? "", r1.Cells[_columnIndex].Value?.ToString() ?? ""));
            }

            _dataGridView.Rows.Clear();
            foreach (var row in rows)
            {
                _dataGridView.Rows.Add((DataGridViewRow)row.Clone());
            }
        }

        /// <summary>
        /// 撤销排序操作
        /// </summary>
        protected override void OnUndo()
        {
            // 恢复原始顺序
            _dataGridView.Rows.Clear();
            foreach (DataGridViewRow row in _originalOrder)
            {
                _dataGridView.Rows.Add((DataGridViewRow)row.Clone());
            }
        }
    }

    /// <summary>
    /// 删除行命令
    /// </summary>
    public class DeleteRowCommand : CommandBase
    {
        private readonly DataGridView _dataGridView;
        private readonly DataGridViewRow[] _deletedRows;
        private readonly int[] _deletedIndices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataGridView">数据网格视图</param>
        /// <param name="rows">要删除的行</param>
        public DeleteRowCommand(DataGridView dataGridView, DataGridViewRow[] rows)
            : base($"删除 {rows.Length} 行")
        {
            _dataGridView = dataGridView ?? throw new ArgumentNullException(nameof(dataGridView));
            _deletedRows = new DataGridViewRow[rows.Length];
            _deletedIndices = new int[rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                _deletedRows[i] = rows[i];
                _deletedIndices[i] = rows[i].Index;
            }
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        protected override void OnExecute()
        {
            foreach (var row in _deletedRows)
            {
                if (!_dataGridView.Rows.Contains(row))
                {
                    _dataGridView.Rows.Remove(row);
                }
            }
        }

        /// <summary>
        /// 撤销删除操作
        /// </summary>
        protected override void OnUndo()
        {
            // 按照原始索引重新插入行
            for (int i = 0; i < _deletedRows.Length; i++)
            {
                var clonedRow = (DataGridViewRow)_deletedRows[i].Clone();
                int insertIndex = Math.Min(_deletedIndices[i], _dataGridView.Rows.Count);
                _dataGridView.Rows.Insert(insertIndex, clonedRow);
            }
        }
    }
}