using CarRantelSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRantelSystem.Controllers
{
    public class RentController : Controller
    {
        SuperCarEntities db = new SuperCarEntities();

        // GET: Rent
        public ActionResult Index()
        {

            var result = (from r in db.rentals
                          join c in db.carregs on r.carid equals c.carno
                          select new RentailViewModel
                          {
                              id = r.id,
                              carid = r.carid,
                              custid = r.custid,
                              fee = r.fee,
                              sdate = r.sdate,
                              edate = r.edate,
                              available = c.available
                          }).ToList();


            return View(result);
        }
        [HttpGet]
        public ActionResult Getcar()
        {
            var car = db.carregs.ToList();
            return Json(car, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Getid(int id)
        {
            var customer = (from s in db.customers where s.id == id select s.custname).ToList();
            return Json(customer, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult Getavil(string carno)
        {
            var caravil = (from s in db.carregs where s.carno ==carno select s.available).FirstOrDefault();

            return Json(caravil, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult Save(rental rent)
        {
            if (ModelState.IsValid)
            {
                db.rentals.Add(rent);
                var car = db.carregs.SingleOrDefault(e => e.carno == rent.carid);
                if (car == null)
                    return HttpNotFound("CarNo Is not Valid");
                car.available = "no";
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rent);
        }

    }
}