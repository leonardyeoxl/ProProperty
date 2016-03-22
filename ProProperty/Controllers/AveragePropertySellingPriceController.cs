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
        public HdbPriceRangeGateway hdbPriceRangeGateway = new HdbPriceRangeGateway();
        public HdbPriceRangeService HdbPriceRangeService;
        public AveragePropertySellingPriceController()
        {
            HdbPriceRangeService = new HdbPriceRangeService(); 
        }

        // GET: AveragePricing
        public ActionResult Index()
        {
            Config(null,null);

            doSynchronization();
            return View(hdbPriceRangeGateway.SelectAll()); 

        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {

            //doSynchronization();

            string district = formCollection["district_DDL"];
            string room = formCollection["roomType_DDL"];
            ViewBag.viewTown = district;
            ViewBag.viewRoom = room;

            Config(district,room);
            //EfficiencyChart(formCollection);
            return View();
            //return RedirectToAction("EfficiencyChart", new { town = district, roomType = room });
        }

        public void doSynchronization()
        {
            hdbPriceRangeGateway.DeleteAllHdbPriceRange();
            List<Hdb_price_range> priceRangeList = new List<Hdb_price_range>();
            priceRangeList = HdbPriceRangeService.GetHdbPriceRange();
            for (int i = 0; i < priceRangeList.Count; i++)
            {
                hdbPriceRangeGateway.Insert(priceRangeList[i]);
            }
        }

        public void Config(string district,string room)
        {
            List<SelectListItem> roomType = new List<SelectListItem>();
            roomType.Add(new SelectListItem() { Text = "2-room" });
            roomType.Add(new SelectListItem() { Text = "3-room" });
            roomType.Add(new SelectListItem() { Text = "4-room" });
            roomType.Add(new SelectListItem() { Text = "5-room" });
            var selectedRoomType = roomType.FirstOrDefault(d => d.Text == room);
            if (selectedRoomType != null)
            {
                selectedRoomType.Selected = true;
            }

            ViewBag.roomType_DDL = roomType;

            List<SelectListItem> districtArea = new List<SelectListItem>();
            districtArea.Add(new SelectListItem() { Text = "Select Area" });
            districtArea.Add(new SelectListItem() { Text = "Punggol" });
            districtArea.Add(new SelectListItem() { Text = "Ang Mo Kio" });
            var selectedDistrict = districtArea.FirstOrDefault(d => d.Text == district);
            if (selectedDistrict != null)
            {
                selectedDistrict.Selected = true;
            }

            ViewBag.district_DDL = districtArea;
        }

        public ActionResult EfficiencyChart(string district, string room)
        {

            var data = hdbPriceRangeGateway.hdbPriceRangeQuery(district, room);

            var myChart = new Chart(width: 1000, height: 600, themePath: "~/Content/ChartHelper.xml")
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

            myChart.Save("~/Content/chart" + district + room, "jpeg");
            // Return the contents of the Stream to the client
            return base.File("~/Content/chart" + district + room, "jpeg");
        }

        public ActionResult compareChart(string district, string room,int currentPrice)
        {

            var data = hdbPriceRangeGateway.hdbPriceRangeQuery(district, room);
            var count = data.Count();
            int[] currentPriceArray = new int[count];
            for (int i = 0; i < count; i++)
            {
                currentPriceArray[i] = currentPrice;
            }
            var myChart = new Chart(width: 1000, height: 600, themePath: "~/Content/ChartHelper.xml")
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
             .AddSeries(
                chartType: "Line",
                name: "Price",
                xValue: data.Select(s => s.financial_year).ToArray(),
                yValues: currentPriceArray)
            .Write();

            myChart.Save("~/Content/chart" + district + room, "jpeg");
            // Return the contents of the Stream to the client
            return base.File("~/Content/chart" + district + room, "jpeg");
        }
    }
}
