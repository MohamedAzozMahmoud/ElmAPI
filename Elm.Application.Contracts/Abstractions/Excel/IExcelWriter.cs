namespace Elm.Application.Contracts.Abstractions.Excel
{
    public interface IExcelWriter
    {
        public MemoryStream WriteExcelFile<T>(IReadOnlyList<T> data, string sheetName);
    }
}
