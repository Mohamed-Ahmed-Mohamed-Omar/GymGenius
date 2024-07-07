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

        public int SubGoalId { get; set; }

        [ForeignKey(nameof(SubGoalId))]
        public SubGoal SubGoal { get; set; }

        public int Current_GoalId { get; set; }

        [ForeignKey(nameof(Current_GoalId))]
        public Current_Goal Current_Goal { get; set; }

        public int Age { get; set; } = default;
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Fat { get; set; }
        public int Level_TrainId { get; set; }

        [ForeignKey(nameof(Level_TrainId))]
        public Level_Train Level_Train { get; set; }

        public string Subscription_Status { get; set; }

        public int Subscription_Duration { get; set; }

        public string Name_Coach { get; set; }

        public DateTime Time_Start { get; set; }

        public DateTime Time_End { get; set; }

        public int Rest { get; set; }

        public int GenderId { get; set; }

        [ForeignKey(nameof(GenderId))]
        public Gender Gender { get; set; }

        public int PlanId { get; set; } = default;

        [ForeignKey(nameof(PlanId))]
        public Plan? Plan { get; set; }
    }
}
