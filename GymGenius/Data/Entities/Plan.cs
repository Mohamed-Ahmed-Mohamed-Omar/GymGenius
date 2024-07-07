using System.ComponentModel.DataAnnotations.Schema;

namespace GymGenius.Data.Entities
{
    public class Plan
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Training_FromId { get; set; }

        [ForeignKey(nameof(Training_FromId))]
        public Training_From Training_From { get; set; }

        public int Current_GoalId { get; set; }

        [ForeignKey(nameof(Current_GoalId))]
        public Current_Goal Current_Goal { get; set; }

        public int Price { get; set; } = default;

        public int Num_of_Quotas { get; set; }
    }
}
