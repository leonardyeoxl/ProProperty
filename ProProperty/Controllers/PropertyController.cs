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
        private static List<PropertyWithPremises> propertyList = new List<PropertyWithPremises>();

        // GET: Property
        public ActionResult Index()
        {
            return View();
        }

        // GET: Property/Details/5
        public ActionResult PropertyDetails(int id)
        {
            foreach(PropertyWithPremises p in propertyList)
            {
                if (p.property.propertyID == id)
                {
                    return View(p);
                }
            }

            return RedirectToAction("Index", "Search");
        }

        public ActionResult PropertyInformation(int id)
        {
            Property propertyObj = propertyDataGateway.SelectById(id);
            int townID = propertyObj.HDBTown;
            Town townName = townDataGateway.SelectById(townID);
            
            if (propertyObj != null)
            {
                ViewBag.Town_Name = townName.town_name; //get town name and store in ViewBag
                ViewBag.Property_Room_Type = propertyObj.GetRoomType().ToString() + "-room"; //get room type and store in ViewBag

                return View(propertyObj);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
            
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