using ProProperty.DAL;
using ProProperty.Models;
using ProProperty.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProProperty.Controllers
{
    public class AveragePropertySellingPriceController : Controller
    {
        public DataGateway<Hdb_price_range> commonDataGateway = new DataGateway<Hdb_price_range>();
        public HdbPriceRangeGateway hdbPriceRangeDataGateway = new HdbPriceRangeGateway();
        public HdbPriceRangeService HdbPriceRange_Gateway;
        public AveragePropertySellingPriceController()
        {
            HdbPriceRange_Gateway = new HdbPriceRangeService(); 
        }

        // GET: AveragePricing
        public ActionResult Index()
        {
            Config();

            doSynchronization();
            return View(commonDataGateway.SelectAll()); 

        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            Config();

            //doSynchronization();

            string district = formCollection["district_DDL"];
            string room = formCollection["roomType_DDL"];
            //EfficiencyChart(formCollection);
            return View(hdbPriceRangeDataGateway.hdbPriceRangeQuery(district, room));
        }

        public void doSynchronization()
        {
            commonDataGateway.DeleteAllHdbPriceRange();
            List<Hdb_price_range> priceRangeList = new List<Hdb_price_range>();
            priceRangeList = HdbPriceRange_Gateway.getHdbPriceRange();
            for (int i = 0; i < priceRangeList.Count; i++)
            {
                commonDataGateway.Insert(priceRangeList[i]);
            }
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

        [HttpPost]
        public ActionResult EfficiencyChart(FormCollection collection)
        {

            //string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
            //+ "FROM Person "
            //+ "WHERE Discriminator = 'Student' "
            //+ "GROUP BY EnrollmentDate";
            //IEnumerable<Hdb_price_range> data = Database.SqlQuery<>(query);
            string district = collection["district_DDL"];
            string room = collection["roomType_DDL"];

            //var data = commonDataGateway.SelectAll();
            //var data = commonDataGateway.SelectAll();
            //data.Select(s => s.financial_year).ToArray();

            var data = hdbPriceRangeDataGateway.hdbPriceRangeQuery(district, room);

            var myChart = new Chart(width: 1000, height: 600)
            .AddTitle(district)
            //.DataBindTable(dataSource: data, xField: "financial_year")
            .AddLegend()
            .AddSeries(
                chartType: "Line",
                name: "Max Selling Price",
                xValue: data.Select(s => s.financial_year).ToArray(),
                yValues: data.Select(s => s.max_selling_price).ToArray())
            .AddSeries(
                chartType: "Line",
                name: "Min Selling Price",
                xValue: data.Select(s => s.financial_year).ToArray(),
                yValues: data.Select(s => s.min_selling_price).ToArray())
            .Write();
            
            myChart.Save("~/Content/chart"+"hello", "jpeg");
            // Return the contents of the Stream to the client
            return base.File("~/Content/chart"+ "hello", "jpeg");
        }
    }
}
