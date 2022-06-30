namespace IsolatedAzureFunctionAppDemo.Services
{
    public interface IData
    {
        public IData Get();
        public string AsJson();
        int? Id { get; set; }
        string? Name { get; set; }
    }
}