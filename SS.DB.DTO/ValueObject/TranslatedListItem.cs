using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
	[Serializable]
	public class TranslatedListItem
	{
		#region Constructor
		public TranslatedListItem()
		{

		}
		#endregion

		#region Property
		public short ID { get; set; }
		public string Symbol { get; set; }
        public string Description { get; set; }
        public string IDSym
        {
            get { return " [ " + Symbol + " ] " + Description; }
        }

        //add by tom 09/03/2009
        //select field Status from DbStatus
        //when AddScalar is datatype must same sing.
        public string strID { get; set; }
        public string strSymbol { get; set; }
        public string strCode { get; set; }
        public string IDSymbol
        {
            get
            {
                return " [ "+ strCode  +" ] " + strSymbol;
            }
        }

		#endregion


	}
}
