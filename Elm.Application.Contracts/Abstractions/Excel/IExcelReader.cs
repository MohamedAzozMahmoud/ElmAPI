namespace Elm.Application.Contracts.Abstractions.Excel
{
    public interface IExcelReader
    {
        public IReadOnlyList<T> ReadExcelFile<T>(Stream stream) where T : new();
    }
}
