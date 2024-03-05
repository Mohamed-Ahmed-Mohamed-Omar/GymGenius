using System.ComponentModel.DataAnnotations.Schema;

namespace GymGenius.Data.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Training_FromId { get; set; }

        [ForeignKey(nameof(Training_FromId))]
        public Training_From Training_From { get; set; }

        public int Current_GoalId { get; set; }

        [ForeignKey(nameof(Current_GoalId))]
        public Current_Goal Current_Goal { get; set; }

        public int Target_MuscleId { get; set; }

        [ForeignKey(nameof(Target_MuscleId))]
        public Target_Muscle Target_Muscle { get; set; }

        public int Age { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Fat { get; set; }
        public int Level_TrainId { get; set; }

        [ForeignKey(nameof(Level_TrainId))]
        public Level_Train Level_Train { get; set; }

        public int TimeId { get; set; }

        [ForeignKey(nameof(TimeId))]
        public Time Time { get; set; }

        public ICollection<SubscriptionDay> SubscriptionDays { get; set; }
    }
}
