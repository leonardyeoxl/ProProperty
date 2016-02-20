using ProProperty.DAL;
using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProProperty.Controllers
{
    public class PropertyController : Controller
    {
        private DataGateway<Property> dataGateway = new DataGateway<Property>();

        static List<Property> model = new List<Property>();

        // GET: Property
        public ActionResult Index()
        {
            List<SelectListItem> priceRange = new List<SelectListItem>();
            priceRange.Add(new SelectListItem() { Text = "Select Max Price"});
            priceRange.Add(new SelectListItem() { Text = "[--- Price Range here --]" });

            ViewBag.priceRange_DDL = priceRange;

            List<SelectListItem> propertyType = new List<SelectListItem>();
            propertyType.Add(new SelectListItem() { Text = "Type of House" });
            propertyType.Add(new SelectListItem() { Text = "[--- House type here --]" });

            ViewBag.propertyType_DDL = propertyType;

            List<SelectListItem> roomType = new List<SelectListItem>();
            roomType.Add(new SelectListItem() { Text = "2" });
            roomType.Add(new SelectListItem() { Text = "3" });
            roomType.Add(new SelectListItem() { Text = "4" });
            roomType.Add(new SelectListItem() { Text = "5" });

            ViewBag.roomType_DDL = roomType;

            List<SelectListItem> districtArea = new List<SelectListItem>();
            districtArea.Add(new SelectListItem() { Text = "Area" });
            districtArea.Add(new SelectListItem() { Text = "[-- District Choices --]" });

            ViewBag.district_DDL = districtArea;

            String[] premiseType_Name = {"School", "Shopping Mall", "Community Club", "Fitness Centre", "Park","Clinic", "MRT Station", "Bus Stop", "Highway", "Petrol Station", "Carpark"};

            List<String> premiseType = new List<String>();
            for(int i=0;i<premiseType_Name.Length;i++)
            {
                premiseType.Add(premiseType_Name[i]);
            }

            var allProperties = dataGateway.getPropertyBasedOnOptions();
            

            ViewBag.PremiseType = premiseType;
            //sample data for testing 
            var property1 = new Property();
            property1.propertyID = 1;
            property1.address = "ang mo kio ave 10";
            property1.Latitude = 1.3699034m;
            property1.Longitude = 103.8454906m;
            property1.propertyType = "hdb";

            model.Add(property1);

            //Property property = dataGateway.SelectById(1);

            return View(allProperties);
        }

        // GET: Property/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Property/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Property/Create
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

        // GET: Property/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Property/Edit/5
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

        // GET: Property/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Property/Delete/5
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
