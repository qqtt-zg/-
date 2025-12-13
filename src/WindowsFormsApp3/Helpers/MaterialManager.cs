using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp3
{
    public class MaterialManager
    {
        private static readonly Lazy<MaterialManager> _instance = new Lazy<MaterialManager>(() => new MaterialManager());
        private List<string> _materials = new List<string>();

        public static MaterialManager Instance => _instance.Value;

        // 添加材料列表变更事件
        public event EventHandler MaterialsChanged;

        public void SetMaterials(List<string> materials)
        {
            _materials = materials ?? new List<string>();
            // 触发材料列表变更事件
            OnMaterialsChanged();
        }

        public void ClearMaterials()
        {
            _materials.Clear();
            // 触发材料列表变更事件
            OnMaterialsChanged();
        }

        public List<string> GetMaterials()
        {
            return new List<string>(_materials);
        }

        // 触发材料变更事件的方法
        protected virtual void OnMaterialsChanged()
        {
            MaterialsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}