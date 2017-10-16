using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library1.Models
{
    public class CheckoutVM
    {
        
            [Key]
            public int id { get; set; }

            public int LoanId { get; set; }
            public string MemberName { get; set; }

            public IEnumerable<SelectListItem> Book { get; set; }

            public IEnumerable<SelectListItem> Member { get; set; }
            public int OnLoan { get; set; }
            public string BookTitle { get; set; }

        [Display(Name = "Checkout Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckoutDate { get; set; }

        [Display(Name = "Returm Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReturnDate { get; set; }

        }
    }

//
