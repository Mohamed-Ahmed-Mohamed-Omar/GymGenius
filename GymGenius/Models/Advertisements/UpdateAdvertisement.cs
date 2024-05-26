using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Models.Advertisements
{
    public class UpdateAdvertisement
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
