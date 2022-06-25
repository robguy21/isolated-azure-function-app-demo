namespace IsolateAzureFunctionApp.Services
{
    public class DataProduction : IData
    {
        public DataProduction()
        {
            Region = "south africa";
        }

        public int? Id { set; get; }
        public string? Name { set; get; }

        public string Region { set; get; }

        public IData Get()
        {
            return this;
        }

        public string AsJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}