namespace GymGenius.Data.Entities
{
    public class Track_Progress
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public float Score { get; set; }

        public int Level { get; set; }

        public DateTime Time { get; set; }
    }
}
