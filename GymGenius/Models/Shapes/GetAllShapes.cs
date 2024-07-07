namespace GymGenius.Models.Shape
{
    public class GetAllShapes
    {
        public string TrainName { get; set; }
        public string? TargetMuscle { get; set; }
        public string? FocusZone { get; set; }
        public string? NumberOfReps { get; set; }
        public string? ExerciseDuration { get; set; }
        public string? ExerciseDescription { get; set; }
        public string LevelName { get; set; }
        public string ImageName { get; set; }
        public string VideoName { get; set; }
        public int countDay { get; set; }
    }
}
