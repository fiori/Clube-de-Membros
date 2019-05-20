using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
                // Specify the directory you want to manipulate.
                string dir = Path.Combine(Server.MapPath("~/Images/uploads/"), members.Name);

                
                // Determine whether the directory exists.
                if (Directory.Exists(dir))
                {
                    Console.WriteLine("That path exists already.");
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(dir);
                    Console.WriteLine("The directory was created successfully at {0}.",
                        Directory.GetCreationTime(dir));
                }

                String newImg = "../../Images/uploads/" + members.Name + "/" + Image.FileName;
                Image.SaveAs(dir + "/" + Image.FileName);
                members.Image = newImg;
                db.Members.Add(members);
                db.SaveChanges();
                return RedirectToAction("Index");
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

            String newImg = "Images/uploads/" + Image.FileName;
            if (newImg != oldImg)
            {
                String path;
                if (oldImg != null)
                {
                    path = Path.Combine(Server.MapPath("~/"), oldImg);
                    System.IO.File.Delete(path);  //Delete old image
                }
                //Gets full path
                path = Path.Combine(Server.MapPath("~/Images/uploads"), Image.FileName);
                //Saves tmp image to final folder
                Image.SaveAs(path);
                //Fills in members object
                return newImg;
            }
            else
            {
                return oldImg;
            }
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
            var path = Path.Combine(Server.MapPath("~/"), members.Image);   //Gets full image path
            System.IO.File.Delete(path);                                    //Deletes image
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
