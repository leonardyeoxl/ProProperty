using ProProperty.DAL;
using ProProperty.Models;
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
        public DataGateway<Hdb_price_range> dataGateway = new DataGateway<Hdb_price_range>();
        public HdbPriceRangeGateway hdbPriceRangeDataGateway = new HdbPriceRangeGateway();
        public HdbPriceRangeService HdbPriceRange_Gateway;
        public AveragePricingController()
        {
            HdbPriceRange_Gateway = new HdbPriceRangeService(); 
        }

        // GET: AveragePricing
        public ActionResult Index()
        {
            Config();

            return View(dataGateway.SelectAll()); 

        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            Config();

            string district = formCollection["district_DDL"];
            string room = formCollection["roomType_DDL"];

            //district = "Punggol";
            //room = "3-room";

            dataGateway.DeleteAllHdbPriceRange();
            List<Hdb_price_range> priceRangeList = new List<Hdb_price_range>();
            priceRangeList = HdbPriceRange_Gateway.getHdbPriceRange();
            for (int i = 0; i < priceRangeList.Count; i++)
            {
                dataGateway.Insert(priceRangeList[i]);
            }

            return View(hdbPriceRangeDataGateway.hdbPriceRangeQuery(district, room));
        }

        public void Config()
        {
            List<SelectListItem> roomType = new List<SelectListItem>();
            roomType.Add(new SelectListItem() { Text = "2-room" });
            roomType.Add(new SelectListItem() { Text = "3-room" });
            roomType.Add(new SelectListItem() { Text = "4-room" });
            roomType.Add(new SelectListItem() { Text = "5-room" });

            ViewBag.roomType_DDL = roomType;

            List<SelectListItem> districtArea = new List<SelectListItem>();
            districtArea.Add(new SelectListItem() { Text = "Select Area" });
            districtArea.Add(new SelectListItem() { Text = "Punggol" });
            districtArea.Add(new SelectListItem() { Text = "Ang Mo Kio" });

            ViewBag.district_DDL = districtArea;
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
