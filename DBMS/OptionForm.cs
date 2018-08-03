using System;
using System.Windows.Forms;
using MongoDB.Driver;

namespace DBMS
{

    public partial class OptionForm : Form
    {
        public static IMongoCollection<PopulationObject> collection;


        public OptionForm()
        {
            InitializeComponent();
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("Population_Dynamics");
            collection = db.GetCollection<PopulationObject>("Pollution");
            ////foreach (PopulationObject populationObject in collection.Find(new BsonDocument()).ToListAsync().Result)
            ////{
            ////    PopulationObject p = collection.Find(x => x.Country.Equals(populationObject.Country)).FirstOrDefaultAsync().Result;
            ////    p.CO2emission *= 10;
            ////    collection.ReplaceOneAsync(x => x.Country.Equals(p.Country), p);
            ////}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SearchForm().ShowDialog();
        }
    }
}
