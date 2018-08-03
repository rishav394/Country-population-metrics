using System;
using System.Windows.Forms;

namespace DBMS
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox && control.Text.Length < 1)
                {
                    MessageBox.Show(
                        "The input format is not quite correct. Please make sure that none of the fields are left blank.",
                        "Hold up!!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }

            try
            {
                OptionForm.collection.InsertOne(new PopulationObject
                {
                    Country = textBox4.Text,
                    CO2emission = double.Parse(textBox1.Text),
                    CO2percent = double.Parse(textBox5.Text),
                    LandArea = double.Parse(textBox2.Text),
                    Population = ulong.Parse(textBox3.Text)
                });
                MessageBox.Show($"Added {textBox4.Text}'s Data!");
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "The input format is not quite correct. Please make sure that the" +
                    " fields get the data they ask for. For example population can only accept numbers.",
                    "Hold up!!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}
