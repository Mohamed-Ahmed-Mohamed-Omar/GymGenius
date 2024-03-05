namespace GymGenius.Models.Subscriptions
{
    public class GetSubscriptionDetails
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Training_From { get; set; }

        public string Current_Goal { get; set; }

        public string Target_Muscle { get; set; }

        public int Age { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }

        public float Fat { get; set; }

        public string Level_Train { get; set; }

        public string Time { get; set; }
    }
}
