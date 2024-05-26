using System.ComponentModel.DataAnnotations.Schema;

namespace GymGenius.Data.Entities
{
    public class SubscriptionDay
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }

        public int DayId { get; set; }

        [ForeignKey(nameof(DayId))]
        public Day Day { get; set; }
    }
}
