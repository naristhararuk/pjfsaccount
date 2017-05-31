using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
    [Serializable]
    public partial class SuProgramRoleSearchResult
    {
        #region Fields

        private short id;
        private bool addState;
        private bool editState;
        private bool deleteState;
        private bool displayState;
        private string comment;
        //private long updBy;
        //private DateTime updDate;
        //private long creBy;
        //private DateTime creDate;
        //private string updPgm;
        //private Byte[] rowVersion;
        private bool active;
        private SS.SU.DTO.SuRole role;
        private SS.SU.DTO.SuProgram program;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Id for the current SuProgramRole
        /// </summary>
        public virtual short Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the AddState for the current SuProgramRole
        /// </summary>
        public virtual bool AddState
        {
            get { return this.addState; }
            set { this.addState = value; }
        }

        /// <summary>
        /// Gets or sets the EditState for the current SuProgramRole
        /// </summary>
        public virtual bool EditState
        {
            get { return this.editState; }
            set { this.editState = value; }
        }

        /// <summary>
        /// Gets or sets the DeleteState for the current SuProgramRole
        /// </summary>
        public virtual bool DeleteState
        {
            get { return this.deleteState; }
            set { this.deleteState = value; }
        }

        /// <summary>
        /// Gets or sets the DisplayState for the current SuProgramRole
        /// </summary>
        public virtual bool DisplayState
        {
            get { return this.displayState; }
            set { this.displayState = value; }
        }

        /// <summary>
        /// Gets or sets the Comment for the current SuProgramRole
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current SuProgramRole
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the Role for the current SuProgramRole
        /// </summary>
        public virtual SS.SU.DTO.SuRole Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        /// <summary>
        /// Gets or sets the Program for the current SuProgramRole
        /// </summary>
        public virtual SS.SU.DTO.SuProgram Program
        {
            get { return this.program; }
            set { this.program = value; }
        }

        #endregion
    }
}
