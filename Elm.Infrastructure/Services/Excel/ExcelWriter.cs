using ClosedXML.Excel;
using Elm.Application.Contracts.Abstractions.Excel;

namespace Elm.Infrastructure.Services.Excel
{
    public class ExcelWriter : IExcelWriter
    {
        public MemoryStream WriteExcelFile<T>(IReadOnlyList<T> data, string sheetName)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(sheetName);

                // Write header
                //for (int col = 1; col <= columnHeaders.Count; col++)
                //{
                //    worksheet.Cell(1, col).Value = columnHeaders[col - 1];
                //}
                worksheet.Cell(1, 1).InsertTable(data); // this will include headers automatically
                worksheet.Rows().Style.Font.FontName = "Arial";
                worksheet.Rows().Style.Font.FontSize = 10;

                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.Row(1).Style.Font.FontSize = 10.5;
                //worksheet.Row(1).Style.Font.FontColor = XLColor.Gray;
                //worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Rows().Style.Protection.SetLocked(true);
                //worksheet.Protect().Protect("MohamedAzoz@200445");

                workbook.SaveAs(stream);
                //stream.Position = 0;
                return stream;
            }

        }


    }
}
