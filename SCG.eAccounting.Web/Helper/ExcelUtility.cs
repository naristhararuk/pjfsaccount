using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

namespace SCG.eAccounting.Web.Helper
{
    public class ExcelUtility
    {
        public static Cell GetCellFromRow(Row r, int columnIndex)
        {
            string cellname = GetExcelColumnName(columnIndex) + r.RowIndex.ToString();
            IEnumerable<Cell> cells = r.Elements<Cell>().Where(x => x.CellReference == cellname);

            return cells.FirstOrDefault();
        }

        public static string GetStringValue(Cell c, WorkbookPart workbookPart)
        {
            string cellValue = string.Empty;

            if (c.DataType != null && c.DataType == CellValues.SharedString)
            {
                int id = -1;

                if (Int32.TryParse(c.InnerText, out id))
                {
                    SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                    return (item.InnerText);
                }

                return null;
            }

            else
                return (c.CellValue == null ? null : c.CellValue.Text);
        }

        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        public static string GetValue(WorkbookPart workbookPart, Row row, int columnIndex)
        {
            return GetStringValue(GetCellFromRow(row, columnIndex), workbookPart);
        }
        public static T GetValue<T>(WorkbookPart workbookPart, Row row, int columnIndex)
        {
            object cellValue = null;

            Cell cell = GetCellFromRow(row, columnIndex);
            string valueString = cell == null ? string.Empty : GetStringValue(cell, workbookPart);

            if (string.IsNullOrEmpty(valueString) && typeof(T) == typeof(Guid))
            {
                cellValue = Guid.Empty;
            }
            else if (!string.IsNullOrEmpty(valueString))
            {
                if (typeof(T) == typeof(Guid) || typeof(T) == typeof(Guid?))
                {
                    cellValue = new Guid(valueString);
                }
                else if (typeof(T) == typeof(decimal) || typeof(T) == typeof(Decimal) || typeof(T) == typeof(decimal?) || typeof(T) == typeof(Decimal?))
                {
                    cellValue = Decimal.Parse(valueString);
                }
                else if (typeof(T) == typeof(short) || typeof(T) == typeof(Int16) || typeof(T) == typeof(short?) || typeof(T) == typeof(Int16?))
                {
                    cellValue = Int16.Parse(valueString);
                }
                else if (typeof(T) == typeof(int) || typeof(T) == typeof(Int32) || typeof(T) == typeof(int?) || typeof(T) == typeof(Int32?))
                {
                    cellValue = Int32.Parse(valueString);
                }
                else if (typeof(T) == typeof(long) || typeof(T) == typeof(Int64) || typeof(T) == typeof(long?) || typeof(T) == typeof(Int64?))
                {
                    cellValue = Int64.Parse(valueString);
                }
                else if (typeof(T) == typeof(DateTimeOffset) || typeof(T) == typeof(DateTimeOffset?))
                {
                    cellValue = new DateTimeOffset(DateTime.FromOADate(double.Parse(valueString)));
                }
                else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
                {
                    cellValue = DateTime.FromOADate(double.Parse(valueString));
                }
                else if (typeof(T) == typeof(bool) || typeof(T) == typeof(Boolean) || typeof(T) == typeof(bool?) || typeof(T) == typeof(Boolean?))
                {
                    cellValue = bool.Parse(valueString);
                }
                else if (typeof(T) == typeof(byte) || typeof(T) == typeof(Byte) || typeof(T) == typeof(byte?) || typeof(T) == typeof(Byte?))
                {
                    cellValue = byte.Parse(valueString);
                }
            }
            return (T)cellValue;
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));
            return string.Format("{0}{1}", firstChar, secondChar);
        }
    }
}