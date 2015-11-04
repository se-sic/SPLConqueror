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
    public partial class FactorizationSettings : Form
    {
        private Dictionary<ConfigurationOption, double> settings;
        private Dictionary<ConfigurationOption, FactorizationOption> panels =
            new Dictionary<ConfigurationOption, FactorizationOption>();

        public FactorizationSettings(Dictionary<ConfigurationOption, double> confs)
        {
            if (confs == null)
                throw new Exception("Parameter confs must not be null!");

            settings = confs;

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

            foreach (ConfigurationOption option in settings.Keys)
            {
                double val = 0;
                settings.TryGetValue(option, out val);

                FactorizationOption panel = new FactorizationOption(option.Name);
                panels.Add(option, panel);

                settingsPanel.Controls.Add(panel);

                panel.upDown.Value = (decimal)val;
                panel.Location = new System.Drawing.Point(0, i * panel.Size.Height);
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
            foreach (KeyValuePair<ConfigurationOption, FactorizationOption> pair in panels)
            {
                FactorizationOption numPan;
                panels.TryGetValue(pair.Key, out numPan);

                settings.Remove(pair.Key);
                settings.Add(pair.Key, (double)numPan.upDown.Value);
            }

            this.Close();
        }

        /// <summary>
        /// This class is used to set the priority of the specified option.
        /// </summary>
        private class FactorizationOption : Panel
        {
            public NumericUpDown upDown = new NumericUpDown();
            public Label label = new Label();

            public FactorizationOption(String s)
            {
                label.Text = s;
                upDown.MouseWheel += UpDown_MouseWheel;

                initializePanel();
            }

            private void UpDown_MouseWheel(object sender, MouseEventArgs e)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }

            private void initializePanel()
            {
                this.Controls.Add(upDown);
                this.Controls.Add(label);
                this.Size = new System.Drawing.Size(222, 28);

                this.label.AutoSize = true;
                this.label.Location = new System.Drawing.Point(6, 6);
                this.label.Name = "label";
                this.label.Size = new System.Drawing.Size(35, 13);

                this.upDown.DecimalPlaces = 1;
                this.upDown.Increment = 0.1M;
                this.upDown.Location = new System.Drawing.Point(167, 4);
                this.upDown.Name = "upDown";
                this.upDown.Size = new System.Drawing.Size(50, 20);
                this.upDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                this.upDown.Value = new decimal(new int[] {
                                1,
                                0,
                                0,
                                0}
                );
            }
        }
    }
}
