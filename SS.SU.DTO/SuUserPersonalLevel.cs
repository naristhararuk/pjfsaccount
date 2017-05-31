using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
     [Serializable]
    public class SuUserPersonalLevel
    {
         public SuUserPersonalLevel()
        {
        }

         public SuUserPersonalLevel(string personalLevel)
		{
            this.personalLevel = personalLevel;
		}
        private string personalLevel;
        public virtual string PersonalLevel
        {
            get { return personalLevel; }
            set { personalLevel = value; }
        }
        private string description;
        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }
        private bool active;
        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }
        private string ordinal;
        public virtual string Ordinal
        {
            get { return ordinal; }
            set { ordinal = value; }
        }
    }
}
