const fs = require('fs');

let model = fs.readFileSync('src/WindowsFormsApp3/Models/MaterialSelectionResult.cs', 'utf8');

// Add SelectedTetBleed to MaterialSelectionResult
const target = 'public string Dimensions { get; set; }';
const replacement = 'public string Dimensions { get; set; }\n        public double SelectedTetBleed { get; set; } = 0;';

model = model.replace(target, replacement);
fs.writeFileSync('src/WindowsFormsApp3/Models/MaterialSelectionResult.cs', model, 'utf8');
console.log('Added SelectedTetBleed to MaterialSelectionResult');
