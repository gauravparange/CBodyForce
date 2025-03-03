using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodyForce
{
    [Table("Attendance")]
    public class Attendance : Audit
    {
        [Key]
        public int AttendanceId  { get; set; }
        public int UserId  { get; set; }
        public DateTime AttendanceDate  { get; set; }
        //public string BiometricData { get; set; }
    }
}
