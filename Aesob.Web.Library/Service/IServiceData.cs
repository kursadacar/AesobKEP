namespace Aesob.Web.Library.Service
{
    public interface IServiceData
    {
        public bool IsEmpty { get; }

        public string Name { get; set; }

        public string Value { get; set; }

        public IReadOnlyCollection<IServiceData> SubData { get; }

        public IServiceData AddSubData(IServiceData data);
    }
}
