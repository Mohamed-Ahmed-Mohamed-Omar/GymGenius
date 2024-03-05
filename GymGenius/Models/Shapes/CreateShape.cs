namespace GymGenius.Models.Shape
{
    public class CreateShape
    {
        public string Name { get; set; }
        public int Training_FromId { get; set; }
        public int Current_GoalId { get; set; }
        public int Target_MuscleId { get; set; }
        public string? ImageName { get; set; }
        public string? VideoName { get; set; }
    }
}
