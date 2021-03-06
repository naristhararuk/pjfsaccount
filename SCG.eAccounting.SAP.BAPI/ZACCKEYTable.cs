
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 2.0
//     Created at 27/4/2552
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
using SAP.Middleware.Connector;
//using SAP.Connector;

namespace SCG.eAccounting.SAP.BAPI
{
    /// <summary>
    /// A typed collection of ZACCKEY elements.
    /// </summary>
    [Serializable]
    public class ZACCKEYTable : BAPITable//: SAPTable
    {

        /// <summary>
        /// Returns the element type ZACCKEY.
        /// </summary>
        /// <returns>The type ZACCKEY.</returns>
        public override Type GetElementType()
        {
            return (typeof(ZACCKEY));
        }

        /// <summary>
        /// Creates an empty new row of type ZACCKEY.
        /// </summary>
        /// <returns>The newZACCKEY.</returns>
        public override object CreateNewRow()
        {
            return new ZACCKEY();
        }

        /// <summary>
        /// The indexer of the collection.
        /// </summary>
        public ZACCKEY this[int index]
        {
            get
            {
                return ((ZACCKEY)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds a ZACCKEY to the end of the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY to be added to the end of the collection.</param>
        /// <returns>The index of the newZACCKEY.</returns>
        public int Add(ZACCKEY value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Inserts a ZACCKEY into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The ZACCKEY to insert.</param>
        public void Insert(int index, ZACCKEY value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Searches for the specified ZACCKEY and returnes the zero-based index of the first occurrence in the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY to locate in the collection.</param>
        /// <returns>The index of the object found or -1.</returns>
        public int IndexOf(ZACCKEY value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Determines wheter an element is in the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY to locate in the collection.</param>
        /// <returns>True if found; else false.</returns>
        public bool Contains(ZACCKEY value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes the first occurrence of the specified ZACCKEY from the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY to remove from the collection.</param>
        public void Remove(ZACCKEY value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Copies the contents of the ZACCKEYTable to the specified one-dimensional array starting at the specified index in the target array.
        /// </summary>
        /// <param name="array">The one-dimensional destination array.</param>           
        /// <param name="index">The zero-based index in array at which copying begins.</param>           
        public void CopyTo(ZACCKEY[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public void SetValue(IRfcTable table)
        {
            this.Clear();

            for (int i = 0; i < table.RowCount; i++)
            {
                table.CurrentIndex = i;
                ZACCKEY dataRow = this.CreateNewRow() as ZACCKEY;
                dataRow.SetValue(table.CurrentRow);
                this.Add(dataRow);
            }
        }

        public IRfcTable GetTable(RfcRepository repository)
        {
            IRfcTable table = repository.GetTableMetadata("ACCKEY_TAB").CreateTable();

            //for (int i = 0; i < List.Count; i++)
            //{
            //    table.Append(((BAPIACRE09)List[i]).GetStructure(repository));
            //}

            return table;
        }

        public IRfcTable GetTable(RfcRepository repository, IRfcTable table)
        {
            for (int i = 0; i < List.Count; i++)
            {
                table.Append(((ZACCKEY)List[i]).GetStructure(repository));
            }

            return table;
        }
    }
}
