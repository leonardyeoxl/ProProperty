using ProProperty.DAL;
using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProProperty.Controllers
{
    public class SearchController : Controller
    {
        private PropertyGateway propertyGateway = new PropertyGateway();
        private PremiseGateway premisesGateway = new PremiseGateway();
        private TownGateway townDataGateway = new TownGateway();
        private PremiseTypeGateway premiseTypeGateway = new PremiseTypeGateway();
        private AgentGateway agentGateway = new AgentGateway();

        static List<PremiseType> premisesTypeList = null;

        private Boolean anyPremisesChecked = false;
        private Boolean allPremisesChecked = true;

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

            foreach (PremiseType type in premisesTypeList)
            {
                type.isChecked = Convert.ToBoolean(formCollection[type.premises_type_name].Split(',')[0]);
            }

            Town town = townDataGateway.SelectByTownName(districtForm);
            if(town == null)
            {
                return RedirectToAction("Index");
            }

            int minPrice = 0, maxPrice = 0;

            if( priceRangeForm == "< 500k")
            {
                minPrice = 0;
                maxPrice = 500000;
            }
            else if (priceRangeForm == "500k - 1m")
            {
                minPrice = 500000;
                maxPrice = 1000000;
            }
            else if (priceRangeForm == "1m - 5m")
            {
                minPrice = 1000000;
                maxPrice = 5000000;
            }
            else
            {
                minPrice = 5000000;
                maxPrice = 10000000;
            }

            int minBuiltSize = Convert.ToInt16(Property.GetMinBuiltSize(roomTypeForm)) - 1; // Round down
            int maxBuiltSize = Convert.ToInt16(Property.GetMaxBuiltSize(roomTypeForm)) + 1; // Round up

            PropertyController.clearListProperty();

            List<Property> allProperties = propertyGateway.GetProperties
                (town.town_id, minPrice, maxPrice, minBuiltSize, maxBuiltSize);

            foreach (Property p in allProperties)
            {
                Agent agt = agentGateway.SelectById(p.agent_id);
                PropertyWithPremises pwp = new PropertyWithPremises() { property = p, agent= agt, listOfPremise = findPremises(p) };
                if (pwp.listOfPremise.Count > 0)
                {
                    PropertyController.addProperty(pwp);
                }
            }

            Config();
            return View("Index", PropertyController.getAllProperties());
        }
        
        public void Config()
        {
            if(premisesTypeList == null)
                premisesTypeList = premiseTypeGateway.SelectAll().Cast<PremiseType>().ToList();

            List<SelectListItem> priceRange = new List<SelectListItem>();
            priceRange.Add(new SelectListItem() { Text = "Select Max Price" });
            priceRange.Add(new SelectListItem() { Text = "< 500k" });
            priceRange.Add(new SelectListItem() { Text = "500k - 1m" });
            priceRange.Add(new SelectListItem() { Text = "1m - 5m" });
            priceRange.Add(new SelectListItem() { Text = "5m >" });

            ViewBag.priceRange_DDL = priceRange;

            List<string> propType = propertyGateway.GetPropertyTypes();
            List<SelectListItem> propertyType = new List<SelectListItem>();
            propertyType.Add(new SelectListItem() { Text = "Select Type of House" });
            foreach(string type in propType)
            {
                propertyType.Add(new SelectListItem() { Text = type });
            }

            ViewBag.propertyType_DDL = propertyType;

            List<SelectListItem> roomType = new List<SelectListItem>();
            roomType.Add(new SelectListItem() { Text = "2" });
            roomType.Add(new SelectListItem() { Text = "3" });
            roomType.Add(new SelectListItem() { Text = "4" });
            roomType.Add(new SelectListItem() { Text = "5" });
            roomType.Add(new SelectListItem() { Text = "Executive Apartment" });

            ViewBag.roomType_DDL = roomType;

            List<Town> towns = townDataGateway.SelectAll().ToList();
            List<SelectListItem> districtArea = new List<SelectListItem>();
            districtArea.Add(new SelectListItem() { Text = "Select Area" });
            foreach(Town t in towns)
            {
                districtArea.Add(new SelectListItem() { Text = t.town_name });
            }

            ViewBag.district_DDL = districtArea;
            ViewBag.PremiseType = premisesTypeList;
        }

        /*distance algo for doing ranging*/
        private double distanceAlgo(Property property, Premise premise)
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

        private double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        private List<Premise> findPremises(Property property)
        {
            List<int> type_id = new List<int>();
            for (int i = 0; i < premisesTypeList.Count; i++)
            {
                anyPremisesChecked |= premisesTypeList[i].isChecked;
                allPremisesChecked &= premisesTypeList[i].isChecked;
                if (premisesTypeList[i].isChecked)
                    type_id.Add(premisesTypeList[i].premises_type_id);
            }

            List<Premise> allPremises = new List<Premise>();
            try
            {
                if (!anyPremisesChecked || allPremisesChecked)
                    allPremises = premisesGateway.SelectAll().ToList();
                else
                    allPremises = premisesGateway.GetPremises(type_id.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                string mes = ex.InnerException.ToString();
            }

            List<Premise> filteredPremises = new List<Premise>();
            foreach (var premise in allPremises)
            {
                if (distanceAlgo(property, premise) < 1.5)
                {
                    filteredPremises.Add(premise);
                }
            }
            return filteredPremises;
        }
    }
}
