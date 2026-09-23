namespace StudentManagment.Models
{
    public class AcademicYear
    {
        public int AcademicYearId { get; set; }

        public string AcademicYearName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}