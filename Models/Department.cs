namespace StudentManagment.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }
    }
}