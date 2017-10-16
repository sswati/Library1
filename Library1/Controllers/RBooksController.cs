using Library1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Library1.Controllers
{
    public class RBooksController : Controller
    {
        public LibraryDb db = new LibraryDb();
        // GET: RBooks
        public ActionResult Index()

        {
            var loans = db.Loans
                .Where(a => a.Book.OnLoan == 1);


            return View(loans);

            // return View(db.Loans.ToList());
        }

        [HttpGet]
        public ActionResult Return(int? id)

        {

            var loanQuery = from item in db.Loans

                            where item.LoanId == id
                            select item;

            


            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Loan bookTable = db.Loans.Find(id);

            if (bookTable == null)

            {

                return HttpNotFound();

            }



            return View(bookTable);
        }



        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult ReturnBook(int id, Loan loan)

        {

            var loanQuery = from item in db.Loans

                            where item.LoanId == id

                            select item;



            if (ModelState.IsValid)

            {

                var book = db.Loans.FirstOrDefault(s => s.LoanId == id);

                if (book != null)

                {
                    book.Book.OnLoan = 0;

                    //book.FinePrice = 0.00M;

                    
                    db.Entry(book).State = EntityState.Modified;
db.Loans.Remove(book);
                    db.SaveChanges();

                    

                    // return RedirectToAction("Index");
                }

            }


            return View();
            // return View("Index");

        }
    }
}

