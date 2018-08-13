using System;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DBMS
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            countryFIlterBox.Enabled = false;
            EmissionFrom.Enabled = false;
            emissionTo.Enabled = false;
            emiPerCapFrom.Enabled = false;
            emiPerCapTo.Enabled = false;
            EmiPerAreaFrom.Enabled = false;
            EmiPerAreaTo.Enabled = false;
            populationFrom.Enabled = false;
            populationTo.Enabled = false;
            AreaFrom.Enabled = false;
            AreaTo.Enabled = false;
            perEmiTo.Enabled = false;
            percEmiFrom.Enabled = false;

            // Setting Default DataBase
            FilterButtonClick(sender, e);
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    $"Are you sure you want to delete the data for {dataGridView1.SelectedRows[e.Row.Index].Cells[0].Value}?",
                    @"Hold Up!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                OptionForm.Collection.DeleteOneAsync(x =>
                    x.Country.Equals(dataGridView1.SelectedRows[e.Row.Index].Cells[0].Value));
            e.Cancel = result != DialogResult.Yes;
        }

        private void FilterButtonClick(object sender, EventArgs e)
        {
            // Check if required textBoxes are not empty
            foreach (Control con in panel3.Controls)
                if (con is TextBox && con.Enabled &&
                    (string.IsNullOrWhiteSpace(con.Text) || string.IsNullOrEmpty(con.Text)))
                {
                    MessageBox.Show(
                        "Please fill up all the required Fields before proceeding." +
                        "\nFor infinity you can use insane Values like \"1e100\"" +
                        "\nUn-check the respective checkboxes to not use the filter.", "Hold up!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

            dataGridView1.Rows.Clear();
            foreach (PopulationObject l in OptionForm.Collection.Find(new BsonDocument()).ToListAsync().Result)
            {
                if (countryCheck.Checked && !l.Country.ToLower().Contains(countryFIlterBox.Text.ToLower())) continue;
                if (emissionCHeck.Checked && !(l.CO2emission > double.Parse(EmissionFrom.Text) &&
                                               l.CO2emission < double.Parse(emissionTo.Text)))
                    continue;
                if (percCheck.Checked && !(l.CO2percent > double.Parse(percEmiFrom.Text) &&
                                           l.CO2percent < double.Parse(perEmiTo.Text)))
                    continue;
                if (areaCheck.Checked &&
                    !(l.LandArea > double.Parse(AreaFrom.Text) && l.LandArea < double.Parse(AreaTo.Text)))
                    continue;
                if (popCheck.Checked && !(l.Population > double.Parse(populationFrom.Text) &&
                                          l.Population < double.Parse(populationTo.Text)))
                    continue;
                if (emissionPerAreaCheck.Checked && !(l.EmissionPerArea > double.Parse(EmiPerAreaFrom.Text) &&
                                                      l.EmissionPerArea < double.Parse(EmiPerAreaTo.Text)))
                    continue;
                if (capitaCheck.Checked && !(l.EmissionPerCapita > double.Parse(emiPerCapFrom.Text) &&
                                             l.EmissionPerCapita < double.Parse(emiPerCapTo.Text)))
                    continue;
                dataGridView1.Rows.Add(l.Country, l.CO2emission, l.CO2percent, l.LandArea, l.Population,
                    l.EmissionPerCapita, l.EmissionPerArea);
            }
        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {
        }

        private void countryCheck_CheckedChanged(object sender, EventArgs e)
        {
            countryFIlterBox.Enabled = countryCheck.Checked;
        }

        private void emissionCHeck_CheckedChanged(object sender, EventArgs e)
        {
            emissionTo.Enabled = emissionCHeck.Checked;
            EmissionFrom.Enabled = emissionCHeck.Checked;
        }

        private void PercCheck_CheckedChanged(object sender, EventArgs e)
        {
            perEmiTo.Enabled = percCheck.Checked;
            percEmiFrom.Enabled = percCheck.Checked;
        }

        private void AreaCheck_CheckedChanged(object sender, EventArgs e)
        {
            AreaFrom.Enabled = AreaTo.Enabled = areaCheck.Checked;
        }

        private void popCheck_CheckedChanged(object sender, EventArgs e)
        {
            populationFrom.Enabled = populationTo.Enabled = popCheck.Checked;
        }

        private void CapitaCheck_CheckedChanged(object sender, EventArgs e)
        {
            emiPerCapFrom.Enabled = emiPerCapTo.Enabled = capitaCheck.Checked;
        }

        private void EmissionPerAreaCheck_CheckedChanged(object sender, EventArgs e)
        {
            EmiPerAreaFrom.Enabled = EmiPerAreaTo.Enabled = emissionPerAreaCheck.Checked;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (double.TryParse(dataGridView1.CurrentCell.Value.ToString(), out double result))
            {
                PopulationObject pop = OptionForm.Collection
                    .Find(x => x.Country.Equals(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0]
                        .Value)).FirstOrDefaultAsync()
                    .Result;
                double d = dataGridView1.Tag is IConvertible convertible ? convertible.ToDouble(null) : 0d;

                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (pop.CO2emission == d)
                {
                    pop.CO2emission = result;
                }
                else if(pop.CO2percent == d)
                {
                    pop.CO2percent = result;
                }
                else if (pop.LandArea == d)
                {
                    pop.LandArea = result;
                }  // ReSharper restore CompareOfFloatsByEqualityOperator
                else if (pop.Population == (ulong)dataGridView1.Tag)
                {
                    pop.Population = (ulong)result;
                }

                OptionForm.Collection.ReplaceOneAsync(x => x.Country.Equals(dataGridView1
                        .Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0]
                        .Value)
                    , pop);
            }
            else
            {
                dataGridView1.CurrentCell.Value = dataGridView1.Tag;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Tag = dataGridView1.CurrentCell.Value;
        }
    }
}