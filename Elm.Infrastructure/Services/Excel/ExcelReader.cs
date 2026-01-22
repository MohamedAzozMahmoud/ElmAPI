using ClosedXML.Excel;
using Elm.Application.Contracts.Abstractions.Excel;
using System.Reflection;

namespace Elm.Infrastructure.Services.Excel
{
    public class ExcelReader : IExcelReader
    {

        public IReadOnlyList<T> ReadExcelFile<T>(Stream stream) where T : new()
        {
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);

            var rows = worksheet.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return new List<T>();
            }

            // Cache property info for better performance
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var headerRow = rows.First();
            var columnMapping = new Dictionary<int, PropertyInfo>();
            for (int colIndex = 1; colIndex <= headerRow.CellsUsed().Count(); colIndex++)
            {
                var headerCell = headerRow.Cell(colIndex);
                var property = properties.FirstOrDefault(p => p.Name.Equals(headerCell.GetString(), StringComparison.OrdinalIgnoreCase));
                if (property != null)
                {
                    columnMapping[colIndex] = property;
                }
            }
            var result = new List<T>(rows.Count - 1);
            foreach (var row in rows)
            {
                if (row.RowNumber() == 1) continue; // Skip header row
                var item = new T();
                foreach (var kvp in columnMapping)
                {
                    var cell = row.Cell(kvp.Key);
                    var property = kvp.Value;
                    var cellValue = GetCellValue(cell, property.PropertyType);
                    if (cellValue != null)
                    {
                        property.SetValue(item, cellValue);
                    }
                }
                result.Add(item);
            }
            return result;


            // Read headers and map column index to property


            // Pre-allocate list with known capacity


            // Read data rows (skip header)


        }

        private static object? GetCellValue(IXLCell cell, Type targetType)
        {
            if (cell.IsEmpty())
                return null;

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                return underlyingType switch
                {
                    Type t when t == typeof(string) => cell.GetString(),
                    Type t when t == typeof(int) => cell.GetValue<int>(),
                    Type t when t == typeof(long) => cell.GetValue<long>(),
                    Type t when t == typeof(double) => cell.GetValue<double>(),
                    Type t when t == typeof(decimal) => cell.GetValue<decimal>(),
                    Type t when t == typeof(DateTime) => cell.GetDateTime(),
                    Type t when t == typeof(bool) => cell.GetBoolean(),
                    Type t when t == typeof(float) => cell.GetValue<float>(),
                    _ => Convert.ChangeType(cell.Value.ToString(), underlyingType)
                };
            }
            catch
            {
                return null;
            }
        }



        //public async Task<IReadOnlyList<T>> ReadExcelFile<T>(Stream stream)
        //{
        //    using var workbook = new XLWorkbook(stream);
        //    var worksheet = workbook.Worksheets.Worksheet(1);
        //    var result = new List<T>();
        //    var rows = worksheet.RowsUsed();
        //    if (rows == null || !rows.Any() || rows.Count() == 1)
        //    {
        //        return result;
        //    }
        //    // Read header
        //    //var numberOfColumns = worksheet.FirstRow().Cells().Count();

        //    // Read data
        //    foreach (var row in rows.Skip(1))
        //    {
        //        var item = Activator.CreateInstance<T>();
        //        foreach (var cell in row.Cells())
        //        {
        //            var property = typeof(T).GetProperty(cell.Address.ColumnNumber.ToString().Trim());
        //            if (property != null && string.IsNullOrEmpty(cell.Value.ToString()))
        //            {
        //                property.SetValue(item, Convert.ChangeType(cell.Value, property.PropertyType));
        //            }
        //        }
        //        result.Add(item);
        //    }

        //    return result;
        //}
    }
}