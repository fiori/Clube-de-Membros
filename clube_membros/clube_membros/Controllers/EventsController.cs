using clube_membros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace clube_membros.Controllers
{
    public class EventsController : Controller
    {
        Events newEvent = new Events
        {
            City = "Lisboa",
            EventDate = DateTime.UtcNow,
            Name = "Primeiro Evento"
        };

        // GET: Event
        public ActionResult Index()
        {
            return View(newEvent);
        }

        public ActionResult Edit()
        {
            return View(newEvent);
        }
    }
}