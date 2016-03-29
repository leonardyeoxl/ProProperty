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
        private PropertyGateway propertyDataGateway = new PropertyGateway();
        private TownGateway townDataGateway = new TownGateway();
        private AgentGateway agentGateway = new AgentGateway();
        private static List<PropertyWithPremises> propertyList = new List<PropertyWithPremises>();

        // GET: Property/Details/5
        public ActionResult PropertyDetails(int? id)
        {
            if (id == null || propertyList.Count <= 0 || propertyList == null)
            {
                return RedirectToAction("Index", "Search");
            }

            foreach (PropertyWithPremises p in propertyList)
            {
                if (p.property.propertyID == id)
                {
                    return View(p);
                }
            }

            return RedirectToAction("Index", "Search");
        }

        public ActionResult PropertyInformation(int? id)
        {
            if(id == null || propertyList.Count <= 0 || propertyList == null)
            {
                return RedirectToAction("Index", "Search");
            }

            foreach (PropertyWithPremises p in propertyList)
            {
                if (p.property.propertyID == id)
                {
                    Town townName = townDataGateway.SelectById(p.property.HDBTown);
                    ViewBag.Town_Name = townName.town_name; //get town name and store in ViewBag
                    ViewBag.Property_Room_Type = p.property.GetRoomType().ToString() + "-room"; //get room type and store in ViewBag
                    ViewBag.CurrentPrice = p.property.asking;
                    return View(p);
                }
            }

            return RedirectToAction("Index", "Search");            
        }

        // Controller public methods
        public static void addProperty(PropertyWithPremises property)
        {
            propertyList.Add(property);
        }

        public static void clearListProperty()
        {
            propertyList.Clear();
        }

        public static IEnumerable<PropertyWithPremises> getAllProperties()
        {
            return propertyList;
        }
    }
}