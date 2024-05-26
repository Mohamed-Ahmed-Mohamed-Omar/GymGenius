namespace GymGenius.Models.Plans
{
    public class UpdatePlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Training_FromId { get; set; }
        public int Current_GoalId { get; set; }
        public int Price { get; set; } = default;
        public int Num_of_Quotas { get; set; }
    }
}
