namespace InspectionNet.Core.Models
{
    public interface IGenApiParameter
    {
        string Name { get; }
        bool IsWritable { get; }

        void NotifyIsWritableChanged();
    }
}
