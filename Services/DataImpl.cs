namespace IsolatedAzureFunctionAppDemo.Services
{
    public class DataImpl : IData
    {
        public int? Id { set; get; }
        public string? Name { set; get; }
        
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