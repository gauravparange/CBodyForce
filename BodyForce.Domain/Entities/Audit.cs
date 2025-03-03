namespace BodyForce
{
    public class Audit
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Boolean IsDeleted { get; set; } = false;

    }
}
