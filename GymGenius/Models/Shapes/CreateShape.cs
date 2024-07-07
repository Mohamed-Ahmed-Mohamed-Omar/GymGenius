namespace GymGenius.Models.Shape
{
    public class CreateShape
    {
        public string TrainName { get; set; }
        public string? TargetMuscle { get; set; }
        public string? FocusZone { get; set; }
        public string? NumberOfReps { get; set; }
        public string? ExerciseDuration { get; set; }
        public string? ExerciseDescription { get; set; }
        public int Training_FromId { get; set; }
        public int Current_GoalId { get; set; }
        public int SubGoalId { get; set; }
        public int  LevelId { get; set; }
        public string? ImageName { get; set; }
        public string? VideoName { get; set; }
        public int DayNumber { get; set; }
    }
}
