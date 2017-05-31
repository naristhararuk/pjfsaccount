using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class TranslatedListItem
    {
        #region Property
        public short ID { get; set; }
        public string Symbol { get; set; }
        #endregion
    }
}
