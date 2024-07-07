using Org.BouncyCastle.Asn1.Esf;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymGenius.Data.Entities
{
    public class Shape_Training
    {
        public int Id { get; set; }
        public string TrainName { get; set; }
        public string? TargetMuscle { get; set; }
        public string? FocusZone { get; set; }
        public string? NumberOfReps { get; set; }
        public string? ExerciseDuration { get; set; }
        public string? ExerciseDescription { get; set; }

        public int Training_FromId { get; set; }

        [ForeignKey(nameof(Training_FromId))]
        public Training_From Training_From { get; set; }

        public int Current_GoalId { get; set; }

        [ForeignKey(nameof(Current_GoalId))]
        public Current_Goal Current_Goal { get; set; }

        public int SubGoalId { get; set; }

        [ForeignKey(nameof(SubGoalId))]
        public SubGoal SubGoal { get; set; }

        public int LevelId { get; set; }

        [ForeignKey(nameof(LevelId))]
        public Level_Train Level_Train { get; set; }

        public string? ImageName { get; set; }

        public string? VideoName { get; set; }

        public int DayNumber { get; set; }
    }
}
