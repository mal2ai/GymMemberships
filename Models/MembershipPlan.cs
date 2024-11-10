namespace GymMemberships.Models
{
    public class MembershipPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
