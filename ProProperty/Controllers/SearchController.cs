using ProProperty.DAL;
using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Text;

namespace ProProperty.Controllers
{
    

    public class SearchController : Controller
    {
        private DataGateway<Property> propertyDataGateway = new DataGateway<Property>();
        private DataGateway<Premise> premisesDataGateway = new DataGateway<Premise>();
        private TownDatagateway townDataGateway = new TownDatagateway();
        //private DataGateway<Hdb_price_range> hdbPriceRangeDataGateway = new DataGateway<Hdb_price_range>();
        private DataGateway<Premises_type> PremisesTypeDataGateway = new DataGateway<Premises_type>();
        String[] premiseType_Name = { "School", "Shopping Mall", "Community Club", "Fitness Centre", "Park", "Clinic", "MRT Station", "Bus Stop", "Highway", "Petrol Station", "Carpark" };

        static List<PremiseTypeCB> premisesCheckBox = null;
        private Boolean anyPremisesChecked = false;

        // GET: Property
        public ActionResult Index()
        {
            Config();

            return View();
        }

        [HttpPost]
        public ActionResult SearchProperty(FormCollection formCollection)
        {
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

            premisesCheckBox[0].isChecked = premisesSchool;
            premisesCheckBox[1].isChecked = premisesShoppingMall;
            premisesCheckBox[2].isChecked = premisesCommunityClub;
            premisesCheckBox[3].isChecked = premisesFitnessCentre;
            premisesCheckBox[4].isChecked = premisesPark;
            premisesCheckBox[5].isChecked = premisesClinic;
            premisesCheckBox[6].isChecked = premisesMRTStation;
            premisesCheckBox[7].isChecked = premisesBusStop;
            premisesCheckBox[8].isChecked = premisesHighway;
            premisesCheckBox[9].isChecked = premisesPetrolStation;
            premisesCheckBox[10].isChecked = premisesCarpark;

            for (int i = 0; i < premisesCheckBox.Count; i++)
            {
                if (premisesCheckBox[i].isChecked)
                {
                    anyPremisesChecked = true;
                    break;
                }
            }

            Town town = townDataGateway.SelectByTownName(districtForm);

            int min = 0, max = 0;

            if( priceRangeForm == "< 500k")
            {
                min = 0;
                max = 500000;
            }
            else if (priceRangeForm == "500k - 1m")
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
                minValue = Math.Round(squareFoot * 45);
                maxValue = Math.Round(squareFoot * 45);
            }
            else if (roomTypeForm == "3")
            {
                minValue = Math.Round(squareFoot * 60);
                maxValue = Math.Round(squareFoot * 65);
            }
            else if (roomTypeForm == "4")
            {
                minValue = Math.Round(squareFoot * 80);
                maxValue = Math.Round(squareFoot * 100);
            }
            else if (roomTypeForm == "5")
            {
                minValue = Math.Round(squareFoot * 110);
                maxValue = Math.Round(squareFoot * 120);
            }

            PropertyController.clearListProperty();
            var allProperties = propertyDataGateway.SelectAll();
            try {
                allProperties = allProperties.Where(
                    property => property.HDBTown == town.town_id &&
                    (property.valuation >= min && property.valuation <= max) &&
                    (property.built_size_in_sqft >= Convert.ToDecimal(minValue) &&
                    property.built_size_in_sqft <= Convert.ToDecimal(maxValue)));
            }
            catch { return RedirectToAction("Index"); }
            foreach (Property p in allProperties)
            {
                if (anyPremisesChecked)
                {
                    PropertyWithPremises pwp = new PropertyWithPremises() { property = p, listOfPremise = findPremises(p) };
                    if (pwp.listOfPremise.Count > 0)
                    {
                        PropertyController.addProperty(pwp);
                    }
                    continue;
                }
                PropertyController.addProperty(new PropertyWithPremises() { property = p });
            }

            Config();
            return View("Index", PropertyController.getAllProperties());
        }

        [HttpPost]
        public ActionResult Sync()
        {
            Config();
            return View("Index");
        }

        

        public void Config()
        {

            List<Premises_type> premisesTypeList = PremisesTypeDataGateway.SelectAll().Cast<Premises_type>().ToList();

            List<SelectListItem> priceRange = new List<SelectListItem>();
            priceRange.Add(new SelectListItem() { Text = "Select Max Price" });
            priceRange.Add(new SelectListItem() { Text = "< 500k" });
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

            if (premisesCheckBox == null)
            {
                premisesCheckBox = new List<PremiseTypeCB>();
                for (int i=0; i< premisesTypeList.Count; i++)
                {
                    premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premisesTypeList[i].premises_type_name, isChecked = false });
                }
                
                /*premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[1], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[2], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[3], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[4], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[5], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[6], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[7], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[8], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[9], isChecked = false });
                premisesCheckBox.Add(new PremiseTypeCB() { premiseType = premiseType_Name[10], isChecked = false });*/
            }

            ViewBag.PremiseType = premisesCheckBox;
        }

        /*distance algo for doing ranging*/
        public double distanceAlgo(Property property, Premise premise)
        {
            double calculation = 0;
            int R = 6371;
            
            double latitude1 = Convert.ToDouble(property.Latitude);
            double latitude2 = Convert.ToDouble(premise.premises_lat);
            double longitude1 = Convert.ToDouble(property.Longitude);
            double longitude2 = Convert.ToDouble(premise.premises_long);

            calculation = R * Math.Acos(Math.Cos(ToRad(latitude1)) * Math.Cos(ToRad(latitude2)) * Math.Cos(ToRad(longitude2) - ToRad(longitude1)) + Math.Sin(ToRad(latitude1)) * Math.Sin(ToRad(latitude2)));
            return calculation;
        }

        public double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public List<Premise> findPremises(Property property)
        {
            List<Premise> allPremises = new List<Premise>();
            try {
                allPremises = premisesDataGateway.SelectAll().ToList();
            }catch(Exception ex)
            {
                string mes = ex.InnerException.ToString();
            }
            List<Premise> filteredPremises = new List<Premise>();
            foreach (var premise in allPremises)
            {
                if(distanceAlgo(property, premise) < 1.5)
                {
                    filteredPremises.Add(premise);
                }
            }
            return filteredPremises;
        }

        // GET: Search/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Search/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Search/Create
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

        // GET: Search/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Search/Edit/5
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

        // GET: Search/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Search/Delete/5
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
