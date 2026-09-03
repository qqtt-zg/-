import os

filepath = 'src/WindowsFormsApp3/Forms/Main/MaterialSelectFormModern.cs'
with open(filepath, 'r', encoding='utf-8-sig') as f:
    content = f.read()

old_fields = ''
        // Ã¡ÀººÅ£Ú³îÇ°öªüÕãÐìÁ÷
        private readonly Dictionary<string, string> _orderRegexDict = new Dictionary<string, string>();
        private OrderNumberMode _currentOrderNumberMode = OrderNumberMode.None;
        private string _selectedOrderRegexName = "";
        private string _selectedOrderRegexPattern = "";
        private bool _isHandlingOrderModeChange = false;
        private bool _isHandlingOrderRegexChange = false;'''.convert_to_cs()
