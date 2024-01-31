using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MilestoneMotorsWeb.Data.Enums;
using MilestoneMotorsWeb.Models;

namespace MilestoneMotorsWeb.ViewModels
{
    public class CreateCarViewModel
    {
        [Required]
        public Condition Condition { get; set; }

        [Required]
        [DisplayName("Car Brand")]
        public Brand Brand { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length of 100 characters")]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [DisplayName("Car Model")]
        public string Model { get; set; }

        [Required]
        [DisplayName("Manufacturing Year")]
        [Range(typeof(int), "1920", "2024", ErrorMessage = "Invalid Manufacturing Year")]
        public int ManufacturingYear { get; set; }

        [Required]
        public string Mileage { get; set; }

        [Required]
        [DisplayName("Body Type")]
        public BodyTypes BodyTypes { get; set; }

        [Required]
        [DisplayName("Fuel Types")]
        public FuelTypes FuelTypes { get; set; }

        [Required]
        [DisplayName("Engine Capacity")]
        public int EngineCapacity { get; set; }

        [Required]
        [DisplayName("Engine Power")]
        public string EnginePower { get; set; }

        [Required]
        [DisplayName("Fixed Price")]
        public YesOrNo FixedPrice { get; set; }

        [Required]
        public YesOrNo Exchange { get; set; }

        [Required]
        [DisplayName("Headliner Image")]
        public IFormFile HeadlinerImageUrl { get; set; }

        [Required(ErrorMessage = "Image required")]
        public IFormFile PhotoOne { get; set; }

        [Required(ErrorMessage = "Image required")]
        public IFormFile PhotoTwo { get; set; }

        [Required(ErrorMessage = "Image required")]
        public IFormFile PhotoThree { get; set; }

        public IFormFile? PhotoFour { get; set; }

        public IFormFile? PhotoFive { get; set; }
        public string? UserId { get; set; }
        public string? AdNumber { get; set; }
    }
}
