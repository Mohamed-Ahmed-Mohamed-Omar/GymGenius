namespace GymGenius.Models.Plans
{
    public class CreatePlan
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelId { get; set; }
        public int Ex_TimeId { get; set; }
        public int Training_FromId { get; set; }
        public int Current_GoalId { get; set; }
        public int Target_MuscleId { get; set; }
    }
}
