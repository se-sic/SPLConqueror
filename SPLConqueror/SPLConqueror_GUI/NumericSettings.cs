using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPLConqueror_GUI
{
    public partial class NumericSettings : Form
    {
        private Dictionary<NumericOption, float> settings;
        private Dictionary<NumericOption, NumericPanel> panels = new Dictionary<NumericOption, NumericPanel>();

        /// <summary>
        /// Constructor of this class.
        /// 
        /// Initializes this form.
        /// </summary>
        /// <param name="nums">Reference to the dictionary of the numeric settings</param>
        public NumericSettings(Dictionary<NumericOption, float> nums)
        {
            if (nums == null)
                throw new ArgumentNullException("Parameter nums must not be null!");

            settings = nums;

            InitializeComponent();
            initializePanels();
        }

        /// <summary>
        /// Initializes the NumericPanels od this form.
        /// 
        /// For each NumericOption there is a panel with a label and a NumericUpDown.
        /// </summary>
        private void initializePanels()
        {
            int i = 0;

            foreach (NumericOption option in settings.Keys)
            {
                float val = 0;
                settings.TryGetValue(option, out val);

                NumericPanel panel = new NumericPanel(option, val);
                panels.Add(option, panel);

                optionPanel.Controls.Add(panel);

                panel.Location = new System.Drawing.Point(0, i*panel.Size.Height);
                i++;
            }
        }

        /// <summary>
        /// Invokes if the OK-Button was pressed.
        /// 
        /// All chosen values of the NumericUpDowns will be added to the dictionary reference of the
        /// numeric values. Afterwards the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<NumericOption, NumericPanel> pair in panels)
            {
                NumericPanel numPan;
                panels.TryGetValue(pair.Key, out numPan);

                settings.Remove(pair.Key);
                settings.Add(pair.Key, (float) numPan.upDown.Value);
            }

            this.Close();
        }

        /// <summary>
        /// This class represents a little panel for the setting of the numeric option.
        /// </summary>
        private class NumericPanel : Panel
        {
            public NumericUpDown upDown;
            public Label label = new System.Windows.Forms.Label();

            /// <summary>
            /// Constructor of this class.
            /// </summary>
            /// <param name="option">NumericOption with information for the componentsy</param>
            public NumericPanel(NumericOption option, double currValue)
            {
                if (option == null)
                    throw new ArgumentNullException("Parameter option must not be null!");

                upDown = new OwnNumericUpDown(option.getAllValues(), currValue);

                initializePanel(option);
            }

            /// <summary>
            /// Initializes this panel with the specified NumericOption.
            /// </summary>
            /// <param name="option">NumericOption with information for the components</param>
            private void initializePanel(NumericOption option)
            {
                // Configuring this panel
                Controls.Add(upDown);
                Controls.Add(label);
                Location = new System.Drawing.Point(0, 0);
                Name = "panel1";
                Size = new System.Drawing.Size(300, 37);
                TabIndex = 1;
                
                // Configuring the label
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(12, 9);
                label.Name = "label";
                label.Size = new System.Drawing.Size(35, 13);
                label.TabIndex = 0;
                label.Text = option.Name;

                // Configuring the NumericUpDown
                upDown.Location = new System.Drawing.Point(216, 7);
                upDown.Name = "upDown";
                upDown.Size = new System.Drawing.Size(75, 20);
                upDown.TabIndex = 1;
            }
        }
        
        /// <summary>
        /// This class is a modified NumericUpDown such that it only displays the values it is given.
        /// </summary>
        private class OwnNumericUpDown : NumericUpDown
        {
            private List<double> values;

            /// <summary>
            /// Constructor of this class.
            /// 
            /// Sets all necessary properties.
            /// </summary>
            /// <param name="l">List of all displayed values</param>
            /// <param name="currValue">Current value that is to be shown</param>
            public OwnNumericUpDown(List<double> l, double currValue)
            {
                if (l == null)
                    throw new ArgumentNullException("Parameter l must not be null!");

                if (!l.Contains(currValue))
                    throw new Exception("The list l does not contain the current value!");

                this.Minimum = (decimal) l.Min();
                this.Maximum = (decimal) l.Max();

                this.Value = (decimal) currValue;

                values = l;
            }

            /// <summary>
            /// 
            /// </summary>
            public override void UpButton()
            {
                if (this.Value != Maximum)
                    this.Value = (decimal) values[values.IndexOf((double) this.Value) + 1];

                UpdateEditText();
            }

            /// <summary>
            /// 
            /// </summary>
            public override void DownButton()
            {
                if (this.Value != Minimum)
                    this.Value = (decimal)values[values.IndexOf((double)this.Value) - 1];

                UpdateEditText();
            }
        }
    }
}
