using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BodyForce
{
    [Table("MemberShip")]
    public class MemberShip : Audit
    {
        [Key]
        public int MemberShipId { get; set; }
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Boolean Status { get; set; }
        public DateTime? RenewalDate { get; set; }

    }
}
