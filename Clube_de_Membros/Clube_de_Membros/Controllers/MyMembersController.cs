using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Clube_de_Membros.Models;

namespace Clube_de_Membros.Controllers
{
    public class MyMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public MembersViewModel GetMembers(int page)
        {
            MembersViewModel viewModel = new MembersViewModel();
            viewModel.currentPage = page;

            //pag 1 -> 0, 2
            //pag 2 -> 3, 5
            //pag 3 -> 6, 8
            //pag x -> (x-1)*rows, ((x-1)*rows)+(rows-1)
            int rows = 6,
                indexBegining = (page - 1) * rows,
                indexEnding = ((page - 1) * rows) + (rows - 1);
            
            List<Members> allMembers = (from m in db.Members
                orderby m.Id ascending
                select m).ToList();
            viewModel.maxPages = Convert.ToInt32(allMembers.Count/rows) + (allMembers.Count % rows!=0?1:0);
            viewModel.filteredMembers = new LinkedList<Members>();
            for (int i = indexBegining; (i <= indexEnding && i < allMembers.Count); i++)
            {
                viewModel.filteredMembers.AddLast(allMembers[i]);
            }

            return viewModel;
        }

        // GET: MyMembers/Index/5
        public ActionResult Index(int id)
        {
            if(id > 0)
                return View(GetMembers(id));
            return View(GetMembers(1));
        }

        // GET: MyMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // GET: MyMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,DateOfBirth,Password")] Members members, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if ((from m in db.Members where m.Email == members.Email select m.Email).Count() > 0)
                {
                    Response.Write("<script>alert('Email already exists!')</script>");
                    return View(members);
                }


                if (string.IsNullOrEmpty(members.Email))
                {
                    Response.Write("<script>alert('Email is empty!')</script>");
                    return View(members);
                }
                try
                {
                    MailAddress to = new MailAddress(members.Email);
                    
                    // Specify the directory you want to manipulate.
                    string dir = Path.Combine(Server.MapPath("~/Images/uploads/"), members.Name);
                    
                    // Determine whether the directory exists.
                    if (Directory.Exists(dir))
                    {
                        //Send response to the user
                        Response.Write("<script>alert('The user already exists!')</script>");
                        //Go back to the page
                        return View(members);
                    }
                    else
                    {
                        // create the directory.
                        DirectoryInfo di = Directory.CreateDirectory(dir);
                        String newImg = "../../Images/uploads/" + members.Name + "/" + Image.FileName;
                        Image.SaveAs(dir + "/" + Image.FileName);
                        members.Image = newImg;
                        db.Members.Add(members);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('Email is not valid!')</script>");
                    return View(members);
                }

                
            }

            return View(members);
        }

        private string UpdatedPicInfo(Members members, HttpPostedFileBase Image)
        {
            IQueryable<String> IQOldImage =
                from m in db.Members
                where m.Id == members.Id
                select m.Image;
            String oldImg = IQOldImage.ToList().ElementAt(0);

            // Specify the directory you want to manipulate.
            string dir = Path.Combine(Server.MapPath("~/Images/uploads/"), members.Name);
            try
            {
                 String newImg = "../../Images/uploads/" + members.Name + "/" + Image.FileName;


                if (newImg != oldImg)
                {

                    if (oldImg != null)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Views/MyMembers/" + oldImg)); //Delete old image
                    }

                    //Saves tmp image to final folder
                    Image.SaveAs(dir + "/" + Image.FileName);
                    //Fills in members object
                    return newImg;
                }
            }
            catch (Exception e)
            {
                return oldImg;
            }

            return oldImg;
        }

        // GET: MyMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: MyMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,DateOfBirth,Password")] Members members, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                members.Image = UpdatedPicInfo(members, Image);
                db.Entry(members).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(members);
        }

        // GET: MyMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return HttpNotFound();
            }
            return View(members);
        }

        // POST: MyMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members members = db.Members.Find(id);
            db.Members.Remove(members);
            db.SaveChanges();
            System.IO.File.Delete(Server.MapPath("~/Views/MyMembers/" + members.Image));                                    //Deletes image
            string root = Path.Combine(Server.MapPath("~/Images/Uploads"), members.Name);//Deletes empty directory
            // If directory does not exist, don't even try   
            if (Directory.Exists(root))
            {
                Directory.Delete(root);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
