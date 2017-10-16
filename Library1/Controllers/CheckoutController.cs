using Library1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library1.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public LibraryDb db = new LibraryDb();

        // GET: Checkout
        public ActionResult Index()

        {

            return View();
        }

        [HttpGet]
        public ActionResult Checkout()

        {


            //  int? MemberId, string name
            //get member's names for drop down list

            // var MemberList = new List<string>();


            // var checkoutdate = db.Loans.Include(g => g.Book.Name);

            var MemberQuery = from item in db.Members

                              select item;

            var AvailableBooks = from item in db.Books.Where(g => g.OnLoan == 0)
                                 select item;


            

            //var AvailableBooks = from item in db.Loans.Where(a => a.ReturnDate > DateTime.Now)
            //                    select item;

            //populate view model to allow data from different tables to be used in same view

            var model = new CheckoutVM

            {

                Member = MemberQuery.Select(a => new SelectListItem

                {

                    Text = a.Name,

                    Value = a.Name

                }),

                Book = AvailableBooks.Select(a => new SelectListItem
                {
                    Text = a.Name,

                    Value = a.Name
                }
                ),

                CheckoutDate = DateTime.Now,

                ReturnDate = DateTime.Now.AddDays(14),



            };



            return View(model);

        }

        [HttpPost]
        public ActionResult CheckoutResult(CheckoutVM model)

        {
            //get member's names for drop down list

            var MemberList = new List<string>();

            var MemberQuery = from item in db.Members

                              select item;

            var AvailableBooks = from item in db.Books.Where(g => g.OnLoan == 0)
                                 select item;


            //populate view model

            var model1 = new CheckoutVM

            {

                Member = MemberQuery.Select(a => new SelectListItem

                {

                    Text = a.Name,

                    Value = a.MemberId.ToString()

                }),

                Book = AvailableBooks.Select(a => new SelectListItem

                {

                    Text = a.Name,

                    Value = a.ISBN.ToString()

                }),

                CheckoutDate = DateTime.Now,

                ReturnDate = DateTime.Now.AddDays(14),



            };

            if (ModelState.IsValid)

            {
                var CheckoutDate = model.CheckoutDate;
                // AvailableBooks. = model.CheckoutDate; create an entity of what you want.....change var to  viewbag....
                var ReturnDate = model.ReturnDate;

                //used to get the instance of the member where the name == name in vm

                var result = db.Members.FirstOrDefault(s => s.Name == model.MemberName);

                //takes the book from the database that is being looked at

                var bookLoan = db.Books.FirstOrDefault(s => s.Name == model.BookTitle);

                var loanID = model.LoanId + 1;


                //Loan table represents the book checked out by selected  member (loan=book and member)

                if (bookLoan != null)

                {

                    bookLoan.OnLoan = 1;

                    Loan loan = new Loan
                    {
                        LoanId = loanID,
                        MemberId = result.MemberId,
                        ISBN = bookLoan.ISBN,
                        ReturnDate = ReturnDate,
                        CheckOutDate = CheckoutDate,
                        FinePrice = 0,


                    };
                    db.Loans.Add(loan);

                    //bookLoan.OnLoan = 1;

                    db.Entry(bookLoan).State = EntityState.Modified;
                    //db.Entry(loan).State = EntityState.Modified;
                    db.SaveChanges();

                }



            }
            return View();


        }

    }

}
   