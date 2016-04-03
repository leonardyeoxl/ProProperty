using ProProperty.DAL;
using ProProperty.Models;
using ProProperty.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProProperty.Controllers
{
    public class AveragePropertySellingPriceController : Controller
    {
        private IHdbPriceRangeGateway hdbPriceRangeGateway = new HdbPriceRangeGateway();
        private IHdbPriceRangeService HdbPriceRangeService;
        private ITownGateway townGateway = new TownGateway();

        private static List<SelectListItem> roomType;
        private static List<SelectListItem> districtArea;

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
            string district = formCollection["district_DDL"];
            string room = formCollection["roomType_DDL"];
            ViewBag.viewTown = district;
            ViewBag.viewRoom = room;

            Config(district,room);
            return View();
        }

        public void doSynchronization()
        {
            hdbPriceRangeGateway.DeleteAllHdbPriceRange();
            List<HdbPriceRange> priceRangeList = new List<HdbPriceRange>();
            priceRangeList = HdbPriceRangeService.GetHdbPriceRange();
            for (int i = 0; i < priceRangeList.Count; i++)
            {
                hdbPriceRangeGateway.Insert(priceRangeList[i]);
            }
        }

        /// <summary>
        /// To setup all configuration for the view layout and store into ViewBag
        /// </summary>
        public void Config(string district,string room)
        {
            if (roomType == null)
            {
                roomType = new List<SelectListItem>();
                string[] types = { "2-room", "3-room", "4-room", "5-room" };
                foreach(string type in types)
                {
                    roomType.Add(new SelectListItem() { Text = type });
                }
            }

            var selectedRoomType = roomType.FirstOrDefault(d => d.Text == room);
            if (selectedRoomType != null)
            {
                selectedRoomType.Selected = true;
            }
            ViewBag.roomType_DDL = roomType;

            if (districtArea == null)
            {
                List<Town> towns = townGateway.SelectAll().ToList();
                districtArea = new List<SelectListItem>();
                foreach(Town town in towns)
                {
                    districtArea.Add(new SelectListItem() { Text = town.town_name });
                }
            }
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

            // Return the contents of the Stream to the client
            if (district == null || room == null) // if null return null chart
            {
                myChart.Save("~/Content/chart", "jpeg");
                return File("~/Content/chart", "jpeg");
            }
            else
            {
                myChart.Save("~/Content/chart_efficiency", "jpeg");
                return File("~/Content/chart_efficiency", "jpeg");
            }
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

            // Return the contents of the Stream to the client
            if(district == null || room == null || currentPrice == 0)
            {
                myChart.Save("~/Content/chart", "jpeg");
                return File("~/Content/chart", "jpeg");
            }
            else
            {
                myChart.Save("~/Content/chart_compare", "jpeg");
                return File("~/Content/chart_compare", "jpeg");
            }
        }
    }
}
