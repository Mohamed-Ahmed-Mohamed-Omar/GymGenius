namespace GymGenius.Models.Subscriptions
{
    public class UpdateSubscription
    {
        public int Id { get; set; }
        public int Training_FromId { get; set; }
        public int Current_GoalId { get; set; }
        public int Target_MuscleId { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Fat { get; set; }
        public int Level_TrainId { get; set; }
        public int TimeId { get; set; }
        public List<int> Days { get; set; }
    }
}
