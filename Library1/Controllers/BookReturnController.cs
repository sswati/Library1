using Library1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Library1.Controllers
{
    public class BookReturnController : Controller
    {
        private LibraryDb db = new LibraryDb();


        //return a list of books on loan

        public ActionResult Index()
        {

            var loans = db.Loans.Where(a => a.Book.OnLoan == 1);


            return View(loans);


        }
        //Find book results-search results on book selection
        [HttpGet]
        public ActionResult LoanSearch(string q)
        {


            var loans = GetLoans(q);
            return PartialView(loans);



        }
        

        public List<Loan> GetLoans(string searchString)
        {


            // if (ModelState.IsValid)
            //  {
            var loan = db.Loans
                  .Where(a => a.Book.Name
                               .Contains(searchString) &&
                              a.Book.OnLoan == 1).ToList();



                return loan;
         //   }
            //  else
            // {
            //     ModelState.AddModelError("", "Something went wrong...");

            //   return new List<Loan> { loan };
            // }

            //create null oblect-pass null object
            //return a different view/partial view











            //error message


        }

    }
}
