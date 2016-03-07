using ProProperty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProProperty.Controllers
{
    public class AveragePricingController : Controller
    {

        internal HdbPriceRangeGateway HdbPriceRange_Gateway;
        internal AveragePricingController()
        {
            HdbPriceRange_Gateway = new HdbPriceRangeGateway(); //create a new QuoteOfTheDayGateway object.
        }

        // GET: AveragePricing
        public ActionResult Index()
        {
            return View();
        }

        // GET: AveragePricing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AveragePricing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AveragePricing/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AveragePricing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AveragePricing/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AveragePricing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AveragePricing/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
