using Microsoft.AspNetCore.Mvc;

namespace GymGenius.Models.Offers
{
    public class UpdateOfffer
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
    }
}
