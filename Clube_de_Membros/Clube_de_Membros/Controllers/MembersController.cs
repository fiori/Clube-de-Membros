using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clube_de_Membros.Models;

namespace Clube_de_Membros.Controllers
{
    public class MembersController : Controller
    {
        private Members mb = new Members
        {
            DateOfBirth = DateTime.UtcNow,
            Email = "fiori94@hotmail.com",
            Name = "Flavio"
               
        };

        // GET: Members
        //[Authorize]
        public ActionResult Index()
        {
            return View(mb);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
        {
            return View(mb);
        }
    }
}