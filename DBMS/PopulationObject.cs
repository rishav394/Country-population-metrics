using MongoDB.Bson.Serialization.Attributes;

namespace DBMS
{
    public class PopulationObject
    {
        [BsonId]
        public string Country { get; set; }
        public double CO2emission { get; set; }
        public double CO2percent { get; set; }
        public double LandArea { get; set; }
        public ulong Population { get; set; }
        public double EmissionPerCapita => CO2emission / Population;
        public double EmissionPerArea => CO2emission / LandArea;
    }
}
