namespace GymGenius.Models.Subscriptions
{
    public class UpdateSubscriptionToUser
    {
        public string UserName { get; set; }
        public int Training_FromId { get; set; }
        public int Current_GoalId { get; set; }
        public int Target_MuscleId { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Fat { get; set; }
        public int Level_TrainId { get; set; }
        public string Subscription_Status { get; set; }
        public int Subscription_Duration { get; set; }
        public string Name_Coach { get; set; }
        public DateTime Time_End { get; set; }
        public int Rest { get; set; }
        public int PlanId { get; set; }
        public int SubGoalId { get; set; }
    }
}
