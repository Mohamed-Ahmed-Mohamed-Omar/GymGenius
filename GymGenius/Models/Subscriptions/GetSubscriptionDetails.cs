namespace GymGenius.Models.Subscriptions
{
    public class GetSubscriptionDetails
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Place { get; set; }
        public string Goal { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Fat { get; set; }
        public int Age { get; set; } = default;
        public string Level_Train { get; set; }
        public string Subscription_Status { get; set; }
        public int Subscription_Duration { get; set; }
        public string Name_Coach { get; set; }
        public DateTime Time_Start { get; set; }
        public DateTime Time_End { get; set; }
        public int Rest { get; set; }
        public string Gender { get; set; }
        public string PlanName { get; set; }
        public string SubGoalName { get; set; }
    }
}
