namespace GymGenius.Models.TrackProgresses
{
    public class GetAllTrackProgress
    {
        public int Level { get; set; }

        public float Score { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}
