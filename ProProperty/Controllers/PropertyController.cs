using Newtonsoft.Json;
using ProProperty.DAL;
using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ProProperty.Controllers
{
    public class JsonResult
    {
        public string help { get; set; }
        public string success { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public string resource_id { get; set; }
        public List<Field> fields { get; set; }
        public List<Record> records { get; set; }
        public Link _links { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
    }

    public class Record
    {
        public string town { get; set; }
        public string max_selling_price { get; set; }
        public string financial_year { get; set; }
        public int _id { get; set; }
        public string max_selling_price_ahg_shg { get; set; }
        public string room_type { get; set; }
        public string min_selling_price { get; set; }
        public string min_selling_price_ahg_shg { get; set; }
    }

    public class Field
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Link
    {
        public string start { get; set; }
        public string next { get; set; }
    }

    public class PropertyController : Controller
    {
        private DataGateway<Property> propertyDataGateway = new DataGateway<Property>();
        private DataGateway<Town> townDataGateway = new DataGateway<Town>();
        
        // GET: Property
        public ActionResult Index()
        {
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

            int min = 0, max = 0;

            if (priceRangeForm == "500k - 1m")
            {
                min = 500000;
                max = 1000000;
            }
            else if (priceRangeForm == "1m - 5m")
            {
                min = 1000000;
                max = 5000000;
            }
            else
            {
                min = 5000000;
                max = 10000000;
            }

            double minValue = 0;
            double maxValue = 0;
            double squareFoot = 10.7639;

            if (roomTypeForm == "2")
            {
                minValue = squareFoot * 45;
                maxValue = squareFoot * 45;
            }
            else if (roomTypeForm == "3")
            {
                minValue = squareFoot * 60;
                maxValue = squareFoot * 65;
            }
            else if (roomTypeForm == "4")
            {
                minValue = squareFoot * 80;
                maxValue = squareFoot * 100;
            }
            else if (roomTypeForm == "5")
            {
                minValue = squareFoot * 110;
                maxValue = squareFoot * 120;
            }

            var allProperties = propertyDataGateway.SelectAll();
            allProperties = allProperties.Where(property => property.HDBTown == town.town_id && (property.valuation >= min && property.valuation <= max) && (property.built_size_in_sqft >= Convert.ToDecimal(minValue) && property.built_size_in_sqft <= Convert.ToDecimal(maxValue)));
            
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

            ViewBag.propertyType_DDL = propertyType;

            List<SelectListItem> roomType = new List<SelectListItem>();
            roomType.Add(new SelectListItem() { Text = "2" });
            roomType.Add(new SelectListItem() { Text = "3" });
            roomType.Add(new SelectListItem() { Text = "4" });
            roomType.Add(new SelectListItem() { Text = "5" });
            roomType.Add(new SelectListItem() { Text = "Executive Apartment" });

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

        /*distance algo for doing ranging*/
        public double distanceAlgo(Property property, Premise premise)
        {
            double calculation = 0;
            int R = 6371000;

            double latitude1 = Convert.ToDouble(property.Latitude);
            double latitude2 = Convert.ToDouble(premise.latitude);
            double longitude1 = Convert.ToDouble(property.Longitude);
            double longitude2 = Convert.ToDouble(premise.longitude);

            double totalLatitude = latitude2 - latitude1;
            double totalLongitude = longitude2 - longitude1;

            double a = Math.Sin(totalLatitude / 2) * Math.Sin(totalLatitude / 2) + Math.Cos(latitude1) * Math.Cos(latitude2) * Math.Sin(totalLongitude/2) * Math.Sin(totalLongitude / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1- a));

            calculation = R * c;
            
            return calculation;
        }

        // GET: Property/Details/5
        public ActionResult PropertyDetails(int id)
        {
            PropertyWithPremises model = new PropertyWithPremises();
            model.property = propertyDataGateway.SelectById(id);
            return View(model);
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

        // Controller public methods
        public void addProperty(PropertyWithPremises property)
        {
            propertyList.Add(property);
        }

        public IEnumerable<PropertyWithPremises> getAllProperties()
        {
            return propertyList;
        }
    }
}
