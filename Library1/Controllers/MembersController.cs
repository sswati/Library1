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
    public class MembersController : Controller
    {
        private LibraryDb db = new LibraryDb();

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,Name,Address,PostCode,TelNo")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,Name,Address,PostCode,TelNo")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //count the number of loans/member
            var numLoans = from xxx in db.Loans

                           where xxx.MemberId == id
                           select xxx;

            int numberOfLoans = numLoans.Count();

            if (numberOfLoans == 0)
            {
                Member member = db.Members.Find(id);
                db.Members.Remove(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("DeleteMember");
            }
            
        }

        public ActionResult DeleteMember()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult DeleteMember(int id)

        //{

        //    //select all members
        //    //var members = db.Members.ToList();


        //    //selects members were they have no loan
        //    //get the member using their id
        //    //Member member = db.Members.Find(id);

        //    //count the number of loans/member
        //    var numLoans = from xxx in db.Loans
                           
        //                   where xxx.MemberId == id
        //                   select xxx;

        //    int numberOfLoans = numLoans.Count();

        //    if (numberOfLoans == 0)
        //    {
        //        return RedirectToAction("Delete", new { id = id });
        //    }
        //    else
        //    {
        //        return RedirectToAction("DeleteMember");
        //    }






        //   // return View();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult QuickSearch(string term)
        {
            var members = GetMembers(term).Select(a => new { value = a.Name });

            return Json(members, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MemberSearch(string qm)
        {
            var members = GetMembers(qm);

            return PartialView(members);


        }
        private List<Member> GetMembers(string searchString)
        {
            return db.Members
                .Where(a => a.Name.Contains(searchString) || a.Address.Contains(searchString))
                .ToList();
        }


       

        
          //  var AvailableBooks = from item in db.Books.Where(g => g.OnLoan == 0)
           //                      select item;


          

        
        //create Book History button in members detail pg
        //for each member select all the books they have rented-loan table
        //ISBN NO =CATegory filtering in the Loans table.If the isbn no is matching choose the last one and return mesage saying this
        //book is onloan else book returned
       



    }
}
