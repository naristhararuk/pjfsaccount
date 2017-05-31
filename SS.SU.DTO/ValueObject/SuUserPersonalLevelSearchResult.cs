using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
    [Serializable]
    class SuUserPersonalLevelSearchResult
    {
        public SuUserPersonalLevelSearchResult()
        {
        }
        private string personalLevel;
        public virtual string PersonalLevel
        {
            get { return this.personalLevel; }
            set { this.personalLevel = value; }
        }
        private string description;
        public virtual string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        private bool active;
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }
    }
}
