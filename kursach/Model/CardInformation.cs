using System.Collections.Generic;

namespace kursach.Model
{
    public class CardInformation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }  

        public string Rental_Period { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }

       
        public virtual ICollection<SliderImage> SliderImages { get; set; }

        public CardInformation()
        {
            SliderImages = new List<SliderImage>();
        }
    }

}
