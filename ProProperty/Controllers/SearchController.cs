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

    public class SearchController : Controller
    {
        private DataGateway<Property> propertyDataGateway = new DataGateway<Property>();
        private DataGateway<Premise> premisesDataGateway = new DataGateway<Premise>();
        private TownDatagateway townDataGateway = new TownDatagateway();
        private DataGateway<Hdb_price_range> hdbPriceRangeDataGateway = new DataGateway<Hdb_price_range>();
        String[] premiseType_Name = { "School", "Shopping Mall", "Community Club", "Fitness Centre", "Park", "Clinic", "MRT Station", "Bus Stop", "Highway", "Petrol Station", "Carpark" };

        static Dictionary<String, Boolean> premisesCheckBox = new Dictionary<string, bool>();
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

            premisesCheckBox.Add(premiseType_Name[0], premisesSchool);
            premisesCheckBox.Add(premiseType_Name[1], premisesShoppingMall);
            premisesCheckBox.Add(premiseType_Name[2], premisesCommunityClub);
            premisesCheckBox.Add(premiseType_Name[3], premisesFitnessCentre);
            premisesCheckBox.Add(premiseType_Name[4], premisesPark);
            premisesCheckBox.Add(premiseType_Name[5], premisesClinic);
            premisesCheckBox.Add(premiseType_Name[6], premisesMRTStation);
            premisesCheckBox.Add(premiseType_Name[7], premisesBusStop);
            premisesCheckBox.Add(premiseType_Name[8], premisesHighway);
            premisesCheckBox.Add(premiseType_Name[9], premisesPetrolStation);
            premisesCheckBox.Add(premiseType_Name[10], premisesCarpark);

            for (int i = 0; i < premisesCheckBox.Count; i++)
            {
                if (premisesCheckBox[premiseType_Name[i]])
                {
                    anyPremisesChecked = true;
                    break;
                }
            }

            Town town = townDataGateway.SelectByTownName(districtForm);

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
            allProperties = allProperties.Where(
                property => property.HDBTown == town.town_id &&
                (property.valuation >= min && property.valuation <= max) &&
                (property.built_size_in_sqft >= Convert.ToDecimal(minValue) &&
                property.built_size_in_sqft <= Convert.ToDecimal(maxValue)));

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

            return View("Index", allProperties);
        }

        [HttpPost]
        public ActionResult Sync()
        {
            Config();
            getHdbPriceRange();
            return View("Index");
        }

        public void getHdbPriceRange()
        {
            string postData = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://data.gov.sg/api/action/datastore_search?resource_id=d23b9636-5812-4b33-951e-b209de710dd5");
            //request.Headers.Add("AccountKey", ACCOUNT_KEY);
            //request.Headers.Add("UniqueUserID", UNIQUE_USER_ID);
            //request.Headers.Add("accept", "application/json");
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            //request.Host = "http://datamall.mytransport.sg";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            System.Diagnostics.Debug.WriteLine("Response stream received. : HDB PRICE RANGE SET DATA " + DateTime.Now.ToString("hh: mm:ss tt"));

            hdbPriceRangeDataGateway.DeleteAllHdbPriceRange();

            String jsonResponse = readStream.ReadToEnd();
            JsonResult jsonResult = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<JsonResult>(jsonResponse);
            foreach (Record i in jsonResult.result.records)
            {
                System.Diagnostics.Debug.WriteLine(i._id);
                System.Diagnostics.Debug.WriteLine(i.town);
                System.Diagnostics.Debug.WriteLine(i.room_type);
                System.Diagnostics.Debug.WriteLine(i.min_selling_price_ahg_shg);
                System.Diagnostics.Debug.WriteLine(i.min_selling_price);

                Hdb_price_range hdbRange = new Hdb_price_range();
                hdbRange.hdb_id = i._id;
                hdbRange.town = i.town;
                hdbRange.room_type = i.room_type;
                hdbRange.min_selling_price_less_ahg_shg = i.min_selling_price_ahg_shg;
                hdbRange.min_selling_price = i.min_selling_price;
                hdbRange.max_selling_price_less_ahg_shg = i.max_selling_price_ahg_shg;
                hdbRange.max_selling_price = i.max_selling_price;
                hdbRange.financial_year = i.financial_year;

                hdbPriceRangeDataGateway.Insert(hdbRange);
                
            }
            response.Close();
            readStream.Close();
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

            double a = Math.Sin(totalLatitude / 2) * Math.Sin(totalLatitude / 2) + Math.Cos(latitude1) * Math.Cos(latitude2) * Math.Sin(totalLongitude / 2) * Math.Sin(totalLongitude / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            calculation = R * c;

            return calculation;
        }

        public List<Premise> findPremises(Property property)
        {
            List<Premise> allPremises = premisesDataGateway.SelectAll().ToList();
            List<Premise> filteredPremises = new List<Premise>();
            foreach (var premise in allPremises)
            {
                if(distanceAlgo(property, premise) <= 1000)
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
