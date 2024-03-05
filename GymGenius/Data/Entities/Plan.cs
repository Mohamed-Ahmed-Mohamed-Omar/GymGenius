using System.ComponentModel.DataAnnotations.Schema;

namespace GymGenius.Data.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelId { get; set; }

        [ForeignKey(nameof(LevelId))]
        public Level_Train Level_Train { get; set; }

        public int Ex_TimeId { get; set; }

        [ForeignKey(nameof(Ex_TimeId))]
        public Time Time { get; set; }

        public int Training_FromId { get; set; }

        [ForeignKey(nameof(Training_FromId))]
        public Training_From Training_From { get; set; }

        public int Current_GoalId { get; set; }

        [ForeignKey(nameof(Current_GoalId))]
        public Current_Goal Current_Goal { get; set; }

        public int Target_MuscleId { get; set; }

        [ForeignKey(nameof(Target_MuscleId))]
        public Target_Muscle Target_Muscle { get; set; }
    }
}
