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
        private IPropertyGateway propertyGateway = new PropertyGateway();
        private IPremiseGateway premisesGateway = new PremiseGateway();
        private ITownGateway townGateway = new TownGateway();
        private IPremiseTypeGateway premiseTypeGateway = new PremiseTypeGateway();
        private IAgentGateway agentGateway = new AgentGateway();
        private IPropertyController propertyController = new PropertyController();

        private static List<PremiseType> premisesTypeList = null;
        private static List<SelectListItem> priceRangeList = null;
        private static List<SelectListItem> propertyTypeList = null;
        private static List<SelectListItem> roomTypeList = null;
        private static List<SelectListItem> districtAreaList = null;
        private static int selectedPriceRange;
        private static int selectedPropertyType;
        private static int selectedRoomType;
        private static int selectedDistrict;

        private Boolean anyPremisesChecked = false;
        private Boolean allPremisesChecked = true;

        // GET: Property
        public ActionResult Index(Boolean? redirect)
        {
            if(redirect == null)
                Config();
            else if ((bool)redirect)
            {
                setError(selectedPriceRange, selectedPropertyType, selectedRoomType, selectedDistrict);
                Config(selectedPriceRange, selectedPropertyType, selectedRoomType, selectedDistrict);
            }
            propertyController.clearListProperty();
            return View();
        }

        
        public ActionResult SearchProperty()
        {
            try {
                selectedPriceRange = Int32.Parse(Request.Form["priceRange_DDL"].ToString());
                selectedPropertyType = Int32.Parse(Request.Form["propertyType_DDL"].ToString());
                selectedRoomType = Int32.Parse(Request.Form["roomType_DDL"].ToString());
                selectedDistrict = Int32.Parse(Request.Form["district_DDL"].ToString());

                foreach (PremiseType type in premisesTypeList)
                {
                    type.isChecked = Convert.ToBoolean(Request.Form[type.premises_type_name].ToString().Split(',')[0]);
                }
            }
            catch
            {
                if (!validateForm(selectedPriceRange, selectedPropertyType, selectedRoomType, selectedDistrict))
                    return RedirectToAction("Index", new { redirect = true });
            }
            
            if(!validateForm(selectedPriceRange,selectedPropertyType,selectedRoomType,selectedDistrict))
            {
                return RedirectToAction("Index", new {redirect = true });
            }

            string priceRangeStr = priceRangeList.Where(price => price.Value == selectedPriceRange.ToString()).First().Text;
            string propertyTypeStr = propertyTypeList.Where(type => type.Value == selectedPropertyType.ToString()).First().Text;
            propertyTypeStr = propertyTypeStr.ToLower();
            string roomTypeStr = roomTypeList.Where(type => type.Value == selectedRoomType.ToString()).First().Text;
            string districtStr = districtAreaList.Where(district => district.Value == selectedDistrict.ToString()).First().Text;

            Town town = townGateway.SelectByTownName(districtStr);
            if(town == null)
            {
                return RedirectToAction("Index");
            }

            int[] minMaxPrice = getMinMaxPrice(priceRangeStr);

            int minBuiltSize = Convert.ToInt16(Property.GetMinBuiltSize(roomTypeStr)) - 1; // Round down
            int maxBuiltSize = Convert.ToInt16(Property.GetMaxBuiltSize(roomTypeStr)) + 1; // Round up

            propertyController.clearListProperty();

            List<Property> allProperties = propertyGateway.GetProperties
                (town.town_id, minMaxPrice[0], minMaxPrice[1], minBuiltSize, maxBuiltSize, propertyTypeStr);

            foreach (Property p in allProperties)
            {
                Agent agt = agentGateway.SelectById(p.agent_id);
                PropertyWithPremises pwp = new PropertyWithPremises() { property = p, agent= agt, listOfPremise = findPremises(p) };
                if (pwp.listOfPremise.Count > 0)
                {
                    propertyController.addProperty(pwp);
                }
            }

            Config(selectedPriceRange, selectedPropertyType, selectedRoomType, selectedDistrict);
            return View("Index", propertyController.getAllProperties());
        }

        /// <summary>
        /// To setup all configuration for the view layout and store into ViewBag
        /// </summary>
        public void Config(int? selectedPriceRange=0, int? selectedPropertyType=0, int? selectedRoomType=0, int? selectedDistrict=0)
        {
            if(premisesTypeList == null)
                premisesTypeList = premiseTypeGateway.SelectAll().Cast<PremiseType>().ToList();

            if (priceRangeList == null)
            {
                int i = 0;
                String[] priceStr = { "Select Max Price", "< 500k", "500k - 1m", "1m - 5m", "5m >" };
                priceRangeList = new List<SelectListItem>();
                foreach(String s in priceStr)
                {
                    priceRangeList.Add(new SelectListItem() { Text = s, Value = (i++).ToString() });
                }
            }
            ViewBag.priceRange_DDL = new SelectList(priceRangeList, "Value", "Text", selectedPriceRange);

            if (propertyTypeList == null)
            {
                int i = 0;
                List<string> propType = propertyGateway.GetPropertyTypes();
                propertyTypeList = new List<SelectListItem>();
                propertyTypeList.Add(new SelectListItem() { Text = "Select House Type", Value = (i++).ToString() });
                foreach (string type in propType)
                {
                    propertyTypeList.Add(new SelectListItem() { Text = type, Value = (i++).ToString() });
                }
            }
            ViewBag.propertyType_DDL = new SelectList(propertyTypeList, "Value", "Text", selectedPropertyType);

            if (roomTypeList == null)
            {
                int i = 0;
                String[] roomTypeStr = { "Select Room Type", "2", "3", "4", "5", "Executive Apartment" };
                roomTypeList = new List<SelectListItem>();
                foreach(String type in roomTypeStr)
                {
                    roomTypeList.Add(new SelectListItem() { Text = type, Value = (i++).ToString() });
                }
            }
            ViewBag.roomType_DDL = new SelectList(roomTypeList, "Value", "Text", selectedRoomType);

            if (districtAreaList == null)
            {
                int i = 0;
                List<Town> towns = townGateway.SelectAll().ToList();
                districtAreaList = new List<SelectListItem>();
                districtAreaList.Add(new SelectListItem() { Text = "Select Area", Value = (i++).ToString() });
                foreach (Town t in towns)
                {
                    districtAreaList.Add(new SelectListItem() { Text = t.town_name, Value = (i++).ToString() });
                }
            }
            ViewBag.district_DDL = new SelectList(districtAreaList, "Value", "Text", selectedDistrict); ;

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

            calculation = R * Math.Acos(Math.Cos(toRad(latitude1)) * Math.Cos(toRad(latitude2)) * Math.Cos(toRad(longitude2) - toRad(longitude1)) + Math.Sin(toRad(latitude1)) * Math.Sin(toRad(latitude2)));
            return calculation;
        }

        private double toRad(double degrees)
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

        // New methods
        private int[] getMinMaxPrice(string priceRange)
        {
            int[] minMaxPrice = new int[2];

            if (priceRange == "< 500k")
            {
                minMaxPrice[0] = 0;
                minMaxPrice[1] = 500000;
            }
            else if (priceRange == "500k - 1m")
            {
                minMaxPrice[0] = 500000;
                minMaxPrice[1] = 1000000;
            }
            else if (priceRange == "1m - 5m")
            {
                minMaxPrice[0] = 1000000;
                minMaxPrice[1] = 5000000;
            }
            else
            {
                minMaxPrice[0] = 5000000;
                minMaxPrice[1] = 10000000;
            }

            return minMaxPrice;
        }

        private Boolean validateForm(int selectedPriceRange, int selectedPropertyType, int selectedRoomType, int selectedDistrict)
        {
            if (selectedPriceRange == 0 || selectedPropertyType == 0 || selectedRoomType == 0 || selectedDistrict == 0)
                return false;
            return true;
        }

        private void setError(int selectedPriceRange, int selectedPropertyType, int selectedRoomType, int selectedDistrict)
        {
            if (selectedPriceRange == 0)
                ViewBag.ErrorPriceRange = true;
            if (selectedPropertyType == 0)
                ViewBag.ErrorPropertyType = true;
            if (selectedRoomType == 0)
                ViewBag.ErrorRoomType = true;
            if (selectedDistrict == 0)
                ViewBag.ErrorDistrict = true;
        }
    }
}
