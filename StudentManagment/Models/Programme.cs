using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagment.Models
{
    public class Programme
    {
        [Key]
        public int ProgrammeId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(20)]
        public string ProgrammeCode { get; set; }

        [Required]
        [StringLength(200)]
        public string ProgrammeName { get; set; }

        [Required]
        [StringLength(50)]
        public string Duration { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }
    }
}