using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class SliderImage
    {
        public int Id { get; set; }  
        public string ImageUrl { get; set; }  
        public int CardInformationId { get; set; }

        public virtual CardInformation CardInformation { get; set; }
    }

}


