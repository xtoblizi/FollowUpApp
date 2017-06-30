namespace FollowUpWebApp.Models
{
    public class GroupCellList
    {
        public int GroupCellListId { get; set; }
        public int MemberId { get; set; }
        public int DepartmentId { get; set; }

        public virtual Member Member { get; set; }
        public virtual Department Department { get; set; }
    }
}