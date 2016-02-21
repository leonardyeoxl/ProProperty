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
        private DataGateway<Property> propertyDataGateway = new DataGateway<Property>();
        private DataGateway<Town> townDataGateway = new DataGateway<Town>();

        static List<Property> model = new List<Property>();

        static List<bool> premisesCheckBox = new List<bool>();

        // GET: Property
        public ActionResult Index()
        {
            Config();

            return View();
        }

        [HttpPost]
        public ActionResult SearchProperty(FormCollection formCollection)
        {
            Config();

            string priceRangeForm = formCollection["priceRange_DDL"];
            string propertyTypeForm = formCollection["propertyType_DDL"];
            string roomTypeForm = formCollection["roomType_DDL"];
            string districtForm = formCollection["district_DDL"];

            bool premisesSchool = Convert.ToBoolean(formCollection["checkbox_PremisesSchool"].Split(',')[0]);
            bool premisesShoppingMall = Convert.ToBoolean(formCollection["checkbox_PremisesShopping Mall"].Split(',')[0]);
            bool premisesCommunityClub = Convert.ToBoolean(formCollection["checkbox_PremisesCommunity Club"].Split(',')[0]);
            bool premisesFitnessCentre = Convert.ToBoolean(formCollection["checkbox_PremisesFitness Centre"].Split(',')[0]);
            bool premisesPark = Convert.ToBoolean(formCollection["checkbox_PremisesPark"].Split(',')[0]);
            bool premisesClinic = Convert.ToBoolean(formCollection["checkbox_PremisesClinic"].Split(',')[0]);
            bool premisesMRTStation = Convert.ToBoolean(formCollection["checkbox_PremisesMRT Station"].Split(',')[0]);
            bool premisesBusStop = Convert.ToBoolean(formCollection["checkbox_PremisesBus Stop"].Split(',')[0]);
            bool premisesHighway = Convert.ToBoolean(formCollection["checkbox_PremisesHighway"].Split(',')[0]);
            bool premisesPetrolStation = Convert.ToBoolean(formCollection["checkbox_PremisesPetrol Station"].Split(',')[0]);
            bool premisesCarpark = Convert.ToBoolean(formCollection["checkbox_PremisesCarpark"].Split(',')[0]);

            premisesCheckBox.Add(premisesSchool);
            premisesCheckBox.Add(premisesShoppingMall);
            premisesCheckBox.Add(premisesCommunityClub);
            premisesCheckBox.Add(premisesFitnessCentre);
            premisesCheckBox.Add(premisesPark);
            premisesCheckBox.Add(premisesClinic);
            premisesCheckBox.Add(premisesMRTStation);
            premisesCheckBox.Add(premisesBusStop);
            premisesCheckBox.Add(premisesHighway);
            premisesCheckBox.Add(premisesPetrolStation);
            premisesCheckBox.Add(premisesCarpark);

            Town town  = townDataGateway.SelectById(districtForm);


            var allProperties = propertyDataGateway.SelectAll();
            allProperties = allProperties.Where(property => property.HDBTown == town.town_id);
            
            return View("Index",allProperties);
        }

        public void Config()
        {
            List<SelectListItem> priceRange = new List<SelectListItem>();
            priceRange.Add(new SelectListItem() { Text = "Select Max Price" });
            priceRange.Add(new SelectListItem() { Text = "500k - 1m" });
            priceRange.Add(new SelectListItem() { Text = "1m - 5m" });
            priceRange.Add(new SelectListItem() { Text = "5m >" });

            ViewBag.priceRange_DDL = priceRange;

            List<SelectListItem> propertyType = new List<SelectListItem>();
            propertyType.Add(new SelectListItem() { Text = "Select Type of House" });
            propertyType.Add(new SelectListItem() { Text = "HDB" });
            propertyType.Add(new SelectListItem() { Text = "Condo" });
            propertyType.Add(new SelectListItem() { Text = "Landed Property" });

            ViewBag.propertyType_DDL = propertyType;

            List<SelectListItem> roomType = new List<SelectListItem>();
            roomType.Add(new SelectListItem() { Text = "2" });
            roomType.Add(new SelectListItem() { Text = "3" });
            roomType.Add(new SelectListItem() { Text = "4" });
            roomType.Add(new SelectListItem() { Text = "5" });

            ViewBag.roomType_DDL = roomType;

            List<SelectListItem> districtArea = new List<SelectListItem>();
            districtArea.Add(new SelectListItem() { Text = "Select Area" });
            districtArea.Add(new SelectListItem() { Text = "Yishun" });
            districtArea.Add(new SelectListItem() { Text = "Ang Mo Kio" });

            ViewBag.district_DDL = districtArea;

            String[] premiseType_Name = { "School", "Shopping Mall", "Community Club", "Fitness Centre", "Park", "Clinic", "MRT Station", "Bus Stop", "Highway", "Petrol Station", "Carpark" };

            List<String> premiseType = new List<String>();
            for (int i = 0; i < premiseType_Name.Length; i++)
            {
                premiseType.Add(premiseType_Name[i]);
            }

            ViewBag.PremiseType = premiseType;
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
