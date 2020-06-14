using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
   public class PageRepository :IPageRepository
    {
        private MyCmsContext db;
        public PageRepository(MyCmsContext context)
        {
            this.db = context;
        }
        public bool DeletePage(Page page)
        {
            try
            {
                db.Entry(page).State = EntityState.Deleted;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePage(int pageId)
        {
            try
            {
                var page = GetPageById(pageId);
                DeletePage(page);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Page> GetAllPages()
        {
            return db.Pages;
        }

        public Page GetPageById(int pageId)
        {
            return db.Pages.Find(pageId);
        }

        public bool InsertPage(Page page)
        {
            try
            {
                db.Pages.Add(page);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void save()
        {
            db.SaveChanges();
        }

        public bool UpdatePage(Page page)
        {
            try
            {
                db.Entry(page).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
