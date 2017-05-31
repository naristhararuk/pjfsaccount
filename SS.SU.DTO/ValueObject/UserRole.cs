using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	[Serializable]
	public class UserRole
	{
		#region Constructor
		public UserRole()
		{

		}
		#endregion

		#region Property
		public long? UserRoleId { get; set; }
		public long? UserId { get; set; }
		public short? RoleId { get; set; }
		public string RoleName { get; set; }
		public string Comment { get; set; }
		public Boolean Active { get; set; }
		#endregion
	}
}
