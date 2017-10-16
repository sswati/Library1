using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library1.Models;

namespace Library1.Controllers
{
    public class BooksController : Controller
    {
        private LibraryDb db = new LibraryDb();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ISBN,Name,Author,Genre,Image,OnLoan")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ISBN,Name,Author,Genre,Image,OnLoan")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //count the number of loans/member
            var numLoans = from xxx in db.Loans

                           where xxx.ISBN == id
                           select xxx;

            int numberOfLoans = numLoans.Count();

            if (numberOfLoans == 0)
            {
                Book books = db.Books.Find(id);
                db.Books.Remove(books);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("DeleteBook");
            }

 }


        //Book book = db.Books.Find(id);
        //db.Books.Remove(book);
        //db.SaveChanges();
        //return RedirectToAction("Index");

        public ActionResult DeleteBook()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Return(int? id)

        {

            var loanQuery = from item in db.Loans

                            where item.ISBN == id

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

        public ActionResult ReturnBook(int id)

        {

            var loanQuery = from item in db.Loans

                            where item.ISBN == id

                            select item;



            if (ModelState.IsValid)

            {

                var book = db.Loans.FirstOrDefault(s => s.ISBN == id);

                if (book != null)

                {
                    book.Book.OnLoan = 0;
                    //book.FinePrice=
                    db.Entry(book).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

            }



            return View();

        }


        public ActionResult QuickSearch(string term)
        {
            var books = GetBooks(term).Select(a => new { value = a.Name });
            return Json(books, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookSearch(string qb)
        {
            var books = GetBooks(qb);

            return PartialView(books);


        }
        private List<Book> GetBooks(string searchString)
        {
            return db.Books
                .Where(a => a.Name.Contains(searchString))
                .ToList();
        }



        
    }
}

