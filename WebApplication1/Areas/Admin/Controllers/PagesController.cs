using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.IO;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        private MyCmsContext db = new MyCmsContext();

        private IPageRepository pageRepository;
        private IPageGroupRepository pageGroupRepository;
        public PagesController()
        {
            pageRepository = new PageRepository(db);
            pageGroupRepository = new PageGroupRepository(db);
        }
        // GET: Admin/Pages
        public ActionResult Index()
        {
            var pages = db.Pages.Include(p => p.PageGroup);
            return View(pages.ToList());
        }

        // GET: Admin/Pages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = pageRepository.GetPageById(id.Value);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // GET: Admin/Pages/Create
        public ActionResult Create()
        {
            ViewBag.GroupID = new SelectList(pageGroupRepository.GetAllGroups(), "GroupID", "GroupTitle");
            return View();
        }

        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PageID,GroupID,Title,ShortDescription,Text,Visit,ImageName,ShowSlider,CreateDate")] Page page , HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if(imgUp != null)
                {
                    page.ImageName = Guid.NewGuid() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/PageImages/" + page.ImageName));
                }
                page.Visit = 0;
                page.CreateDate = DateTime.Now;
                pageRepository.InsertPage(page);
                pageRepository.save();
                return RedirectToAction("Index");
            }

            ViewBag.GroupID = new SelectList(pageGroupRepository.GetAllGroups(), "GroupID", "GroupTitle", page.GroupID);
            return View(page);
        }

        // GET: Admin/Pages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = pageRepository.GetPageById(id.Value);
            if (page == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupID = new SelectList(pageGroupRepository.GetAllGroups(), "GroupID", "GroupTitle", page.GroupID);
            return View(page);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PageID,GroupID,Title,ShortDescription,Text,Visit,ImageName,ShowSlider,CreateDate")] Page page , HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                page.Visit = 0;
                page.CreateDate = DateTime.Now;
                if (imgUp != null)
                {
                    if(page.ImageName != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/PageImages/" + page.ImageName));
                    }

                    page.ImageName = Guid.NewGuid() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/PageImages/" + page.ImageName));
                }
                pageRepository.UpdatePage(page);
                pageRepository.save();
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = new SelectList(pageGroupRepository.GetAllGroups(), "GroupID", "GroupTitle", page.GroupID);
            return View(page);
        }

        // GET: Admin/Pages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = pageRepository.GetPageById(id.Value);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var page = pageRepository.GetPageById(id);
            if(page.ImageName != null)
            {
                System.IO.File.Delete(Server.MapPath("/PageImages/" + page.ImageName));
            }
            pageRepository.DeletePage(page);
            pageRepository.save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                pageRepository.Dispose();
                pageGroupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
