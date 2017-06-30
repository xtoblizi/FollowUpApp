using System.Collections.Generic;

namespace FollowUpWebApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public DeptType DepartmenType { get; set; }

        public virtual ICollection<GroupCellList> GroupCellLists { get; set; }
    }
}