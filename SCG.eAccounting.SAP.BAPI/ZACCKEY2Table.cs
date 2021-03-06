
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
using SAP.Middleware.Connector;
//using SAP.Connector;

namespace SCG.eAccounting.SAP.BAPI
{
    /// <summary>
    /// A typed collection of ZACCKEY2 elements.
    /// </summary>
    [Serializable]
    public class ZACCKEY2Table : BAPITable//: SAPTable
    {

        /// <summary>
        /// Returns the element type ZACCKEY2.
        /// </summary>
        /// <returns>The type ZACCKEY2.</returns>
        public override Type GetElementType()
        {
            return (typeof(ZACCKEY2));
        }

        /// <summary>
        /// Creates an empty new row of type ZACCKEY2.
        /// </summary>
        /// <returns>The newZACCKEY2.</returns>
        public override object CreateNewRow()
        {
            return new ZACCKEY2();
        }

        /// <summary>
        /// The indexer of the collection.
        /// </summary>
        public ZACCKEY2 this[int index]
        {
            get
            {
                return ((ZACCKEY2)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds a ZACCKEY2 to the end of the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY2 to be added to the end of the collection.</param>
        /// <returns>The index of the newZACCKEY2.</returns>
        public int Add(ZACCKEY2 value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Inserts a ZACCKEY2 into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The ZACCKEY2 to insert.</param>
        public void Insert(int index, ZACCKEY2 value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Searches for the specified ZACCKEY2 and returnes the zero-based index of the first occurrence in the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY2 to locate in the collection.</param>
        /// <returns>The index of the object found or -1.</returns>
        public int IndexOf(ZACCKEY2 value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Determines wheter an element is in the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY2 to locate in the collection.</param>
        /// <returns>True if found; else false.</returns>
        public bool Contains(ZACCKEY2 value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes the first occurrence of the specified ZACCKEY2 from the collection.
        /// </summary>
        /// <param name="value">The ZACCKEY2 to remove from the collection.</param>
        public void Remove(ZACCKEY2 value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Copies the contents of the ZACCKEY2Table to the specified one-dimensional array starting at the specified index in the target array.
        /// </summary>
        /// <param name="array">The one-dimensional destination array.</param>           
        /// <param name="index">The zero-based index in array at which copying begins.</param>           
        public void CopyTo(ZACCKEY2[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public void SetValue(IRfcTable table)
        {
            this.Clear();

            for (int i = 0; i < table.RowCount; i++)
            {
                table.CurrentIndex = i;
                ZACCKEY2 dataRow = this.CreateNewRow() as ZACCKEY2;
                dataRow.SetValue(table.CurrentRow);
                this.Add(dataRow);
            }
        }

        public IRfcTable GetTable(RfcRepository repository)
        {
            IRfcTable table = repository.GetTableMetadata("ACCKEY_TAB").CreateTable(List.Count);

            for (int i = 0; i < List.Count; i++)
            {
                table.Append(((ZACCKEY2)List[i]).GetStructure(repository));
            }

            return table;
        }
    }
}
