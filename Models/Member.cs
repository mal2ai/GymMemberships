namespace GymMemberships.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }

        // Membership Plan ID (this is not a foreign key but will be used to store the selected plan ID)
        public int MembershipPlanId { get; set; }

    }
}
