
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 2.0
//     Created at 2/4/2552
//     Created from Windows
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// 
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
//using SAP.Connector;
using SAP.Middleware.Connector;

namespace SCG.eAccounting.SAP.BAPI
{
    /// <summary>
    /// A typed collection of BAPIACRE09 elements.
    /// </summary>
    [Serializable]
    public class BAPIACRE09Table : BAPITable//: SAPTable
    {

        /// <summary>
        /// Returns the element type BAPIACRE09.
        /// </summary>
        /// <returns>The type BAPIACRE09.</returns>
        public override Type GetElementType()
        {
            return (typeof(BAPIACRE09));
        }

        /// <summary>
        /// Creates an empty new row of type BAPIACRE09.
        /// </summary>
        /// <returns>The newBAPIACRE09.</returns>
        public override object CreateNewRow()
        {
            return new BAPIACRE09();
        }

        /// <summary>
        /// The indexer of the collection.
        /// </summary>
        public BAPIACRE09 this[int index]
        {
            get
            {
                return ((BAPIACRE09)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds a BAPIACRE09 to the end of the collection.
        /// </summary>
        /// <param name="value">The BAPIACRE09 to be added to the end of the collection.</param>
        /// <returns>The index of the newBAPIACRE09.</returns>
        public int Add(BAPIACRE09 value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Inserts a BAPIACRE09 into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The BAPIACRE09 to insert.</param>
        public void Insert(int index, BAPIACRE09 value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Searches for the specified BAPIACRE09 and returnes the zero-based index of the first occurrence in the collection.
        /// </summary>
        /// <param name="value">The BAPIACRE09 to locate in the collection.</param>
        /// <returns>The index of the object found or -1.</returns>
        public int IndexOf(BAPIACRE09 value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Determines wheter an element is in the collection.
        /// </summary>
        /// <param name="value">The BAPIACRE09 to locate in the collection.</param>
        /// <returns>True if found; else false.</returns>
        public bool Contains(BAPIACRE09 value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes the first occurrence of the specified BAPIACRE09 from the collection.
        /// </summary>
        /// <param name="value">The BAPIACRE09 to remove from the collection.</param>
        public void Remove(BAPIACRE09 value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Copies the contents of the BAPIACRE09Table to the specified one-dimensional array starting at the specified index in the target array.
        /// </summary>
        /// <param name="array">The one-dimensional destination array.</param>           
        /// <param name="index">The zero-based index in array at which copying begins.</param>           
        public void CopyTo(BAPIACRE09[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public void SetValue(IRfcTable table)
        {
            this.Clear();

            for (int i = 0; i < table.RowCount; i++)
            {
                table.CurrentIndex = i;
                BAPIACRE09 dataRow = this.CreateNewRow() as BAPIACRE09;
                dataRow.SetValue(table.CurrentRow);
                this.Add(dataRow);
            }
        }

        public IRfcTable GetTable(RfcRepository repository)
        {
            IRfcTable table = repository.GetTableMetadata("REALESTATE").CreateTable(List.Count);

            for (int i = 0; i < List.Count; i++)
            {
                table.Append(((BAPIACRE09)List[i]).GetStructure(repository));
            }

            return table;
        }

        public IRfcTable GetTable(RfcRepository repository, IRfcTable table)
        {
            for (int i = 0; i < List.Count; i++)
            {
                table.Append(((BAPIACRE09)List[i]).GetStructure(repository));
            }

            return table;
        }
    }
}
