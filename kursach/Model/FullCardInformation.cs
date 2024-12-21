using System.Collections.Generic;

namespace kursach.Model
{
    public class FullCardInformationModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } 

        public List<SliderImage> SliderImages { get; set; }

        public string Rental_Period { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }

        public FullCardInformationModel()
        {
            SliderImages = new List<SliderImage>();
        }
    }
}
