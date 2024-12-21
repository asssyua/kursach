using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Model
{
    public class UserRequestFull
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserFullName { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string RentalTime { get; set; }
        public bool Status { get; set; }

        public string Card_Title { get; set; }
        public string Card_Description { get; set; }
        public string Card_ImageUrl { get; set; }

        public string Card_Rental_Period { get; set; }
        public decimal Card_Price { get; set; }
        public string Card_Location { get; set; }

        public string StatusDisplay => Status ? "Одобрена" : "Не одобрена";
    }
}
