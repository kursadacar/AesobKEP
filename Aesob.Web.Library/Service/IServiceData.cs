namespace Aesob.Web.Library.Service
{
    public interface IServiceData
    {
        bool IsEmpty { get; }

        bool IsDirty { get; set; }

        string Name { get; set; }

        string Value { get; set; }

        IReadOnlyCollection<IServiceData> SubData { get; }

        IServiceData AddSubData(IServiceData data);
    }

    public struct DataAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
