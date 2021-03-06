
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
    /// A typed collection of BAPIACEXTC elements.
    /// </summary>
    [Serializable]
    public class BAPIACEXTCTable : BAPITable//: SAPTable
    {

        /// <summary>
        /// Returns the element type BAPIACEXTC.
        /// </summary>
        /// <returns>The type BAPIACEXTC.</returns>
        public override Type GetElementType()
        {
            return (typeof(BAPIACEXTC));
        }

        /// <summary>
        /// Creates an empty new row of type BAPIACEXTC.
        /// </summary>
        /// <returns>The newBAPIACEXTC.</returns>
        public override object CreateNewRow()
        {
            return new BAPIACEXTC();
        }

        /// <summary>
        /// The indexer of the collection.
        /// </summary>
        public BAPIACEXTC this[int index]
        {
            get
            {
                return ((BAPIACEXTC)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds a BAPIACEXTC to the end of the collection.
        /// </summary>
        /// <param name="value">The BAPIACEXTC to be added to the end of the collection.</param>
        /// <returns>The index of the newBAPIACEXTC.</returns>
        public int Add(BAPIACEXTC value)
        {
            return List.Add(value);
        }

        /// <summary>
        /// Inserts a BAPIACEXTC into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The BAPIACEXTC to insert.</param>
        public void Insert(int index, BAPIACEXTC value)
        {
            List.Insert(index, value);
        }

        /// <summary>
        /// Searches for the specified BAPIACEXTC and returnes the zero-based index of the first occurrence in the collection.
        /// </summary>
        /// <param name="value">The BAPIACEXTC to locate in the collection.</param>
        /// <returns>The index of the object found or -1.</returns>
        public int IndexOf(BAPIACEXTC value)
        {
            return List.IndexOf(value);
        }

        /// <summary>
        /// Determines wheter an element is in the collection.
        /// </summary>
        /// <param name="value">The BAPIACEXTC to locate in the collection.</param>
        /// <returns>True if found; else false.</returns>
        public bool Contains(BAPIACEXTC value)
        {
            return List.Contains(value);
        }

        /// <summary>
        /// Removes the first occurrence of the specified BAPIACEXTC from the collection.
        /// </summary>
        /// <param name="value">The BAPIACEXTC to remove from the collection.</param>
        public void Remove(BAPIACEXTC value)
        {
            List.Remove(value);
        }

        /// <summary>
        /// Copies the contents of the BAPIACEXTCTable to the specified one-dimensional array starting at the specified index in the target array.
        /// </summary>
        /// <param name="array">The one-dimensional destination array.</param>           
        /// <param name="index">The zero-based index in array at which copying begins.</param>           
        public void CopyTo(BAPIACEXTC[] array, int index)
        {
            List.CopyTo(array, index);
        }

        public void SetValue(IRfcTable table)
        {
            this.Clear();

            for (int i = 0; i < table.RowCount; i++)
            {
                table.CurrentIndex = i;
                BAPIACEXTC dataRow = this.CreateNewRow() as BAPIACEXTC;
                dataRow.SetValue(table.CurrentRow);
                this.Add(dataRow);
            }
        }

        public IRfcTable GetTable(RfcRepository repository)
        {
            IRfcTable table = repository.GetTableMetadata("EXTENSION1").CreateTable(List.Count);

            for (int i = 0; i < List.Count; i++)
            {
                table.Append(((BAPIACEXTC)List[i]).GetStructure(repository));
            }

            return table;
        }

        public IRfcTable GetTable(RfcRepository repository, IRfcTable table)
        {
            for (int i = 0; i < List.Count; i++)
            {
                table.Append(((BAPIACEXTC)List[i]).GetStructure(repository));
            }

            return table;
        }
    }
}
