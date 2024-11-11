namespace GymMemberships.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }

        // Membership Plan ID
        public int MembershipPlanId { get; set; }

        // New attributes
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } 
    }
}
