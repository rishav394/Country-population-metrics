using System;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DBMS
{
    public partial class OptionForm : Form
    {
        public static IMongoCollection<PopulationObject> Collection;

        public OptionForm()
        {
            InitializeComponent();
            MongoMethod();
        }

        private void MongoMethod()
        {
            Console.WriteLine(@"Mongo DB Application");
            Console.WriteLine(@"====================================================");
            Console.WriteLine(@"Configuration Setting: mongodb://localhost:27017");
            Console.WriteLine(@"====================================================");
            Console.WriteLine(@"Initializing connection");
            Console.WriteLine(@"Creating Client..........");

            var client = new MongoClient();

            Console.WriteLine(@"Client Created Successfully........");
            Console.WriteLine($@"Client: {client}");
            Console.WriteLine(@"Initiating Mongo Database.........");

            var db = client.GetDatabase("Population_Dynamics");
            var isMongoLive = db.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait(1000);
            if (isMongoLive)
            {
                Console.WriteLine(@"MongoDB is running......");
                Console.WriteLine(@"Getting reference of database.......");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"MongoDB is not running. Trying to start it.....");

                if (!IsSoftwareInstalled("Mongo"))
                {
                    Console.WriteLine(
                        @"Looks like MongoDB is not installed. Please install it manually.....");
                    Console.ResetColor();
                    Environment.Exit(1);
                }

                try
                {
                    Console.ResetColor();
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo("C:\\Program Files\\MongoDB\\Server\\4.0\\bin\\mongod.exe")
                        {
                            UseShellExecute = false
                        }
                    };

                    p.Start();
                    Thread.Sleep(10000);
                    Console.WriteLine(@"

Retrying to connect to MongoDB...................

");
                    MongoMethod();
                    return;
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        @"Looks like MongoDB is installed but in a different location.
                        Please start mongoDB manually.....");
                }

                Console.ResetColor();
                Environment.Exit(1);
            }

            var collectionName = "Pollution";
            var filter = new BsonDocument("name", collectionName);
            var collections = db.ListCollectionsAsync(new ListCollectionsOptions {Filter = filter});
            var collectionExists = collections.Result.Any();
            if (collectionExists)
            {
                Console.WriteLine(@"Existing collection exists......");
            }
            else
            {
                Console.WriteLine(@"No existing collection was found. Make a new collection? [Y/N]");
                var resp = Console.ReadKey();
                if (resp.KeyChar == 'Y' || resp.KeyChar == 'y')
                {
                    db.CreateCollection(collectionName);
                }
                else
                {
                    Console.WriteLine(@"Okay exiting.......");
                    Environment.Exit(1);
                }
            }

            Collection = db.GetCollection<PopulationObject>(collectionName);

            Console.WriteLine($@"Connected to {Collection.CollectionNamespace}");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new AddForm().ShowDialog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            new SearchForm().ShowDialog();
        }

        public static bool IsSoftwareInstalled(string softwareName)
        {
            var searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_Product");

            foreach (var queryObj in searcher.Get())
                if (queryObj["Caption"].ToString().Contains(softwareName))
                    return true;

            return false;
        }
    }
}