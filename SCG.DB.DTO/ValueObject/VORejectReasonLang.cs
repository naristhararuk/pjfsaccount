using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class VORejectReasonLang
    {
        public string ReasonFormat
        {
            get
            {
                if (string.IsNullOrEmpty(ReasonDetail))
                {
                    return string.Format("[{0}]-{1}", ReasonCode, string.Empty);
                }
                else
                {
                    return string.Format("[{0}]-{1}", ReasonCode, ReasonDetail);
                }
            }
        }
        public int?   ReasonID          { get; set; }
        public string   ReasonCode          { get; set; }
        public string   LanguageName        { get; set; }
        public string   DocumentTypeCode        { get; set; }

        public short?   LanguageID          { get; set; }
        public string   ReasonDetail      { get; set; }

        public string   Comment         { get; set; }
        public bool     Active          { get; set; }

        public string StateEventID { get; set; }
        public bool? RequireComment { get; set; }
        public bool? RequireConfirmReject { get; set; }

        public int? WorkFlowStateEventID { get; set; }

        public VORejectReasonLang()
        {
        }
    }
}
