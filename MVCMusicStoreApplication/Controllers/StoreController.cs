using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCMusicStoreApplication.Models;

namespace MVCMusicStoreApplication.Controllers
{
    public class StoreController : Controller
    {
        private MVCMusicStoreDB db = new MVCMusicStoreDB();


      // GET: Browse
        public ActionResult Browse()
        {
            var genres = GetGenres();
            return View("Browse", genres);
            
        }

        private List<Genre> GetGenres()
        {
            return db.Genres
                .ToList();
        }

        // GET: Details
        public ActionResult Details(int id)
        {
            var details = GetDetails(id);
            return View("Details", details);
        }

        private object GetDetails(int id2)
        {
            return db.Albums
                .Where(a => a.AlbumId.Equals(id2))
                .First();
        }
       
        //Alternate Button code
        /*<button onclick="location.href='@Url.Action("Cart",new { })'">
        Add to Cart
        </button>*/
        
            // GET: Index
        public ActionResult Index(int id)
        {
            var albums = GetAlbums(id);
            return View("Index", albums);
        }

        private List<Album> GetAlbums(int id1)
        {
            return db.Albums
                .Where(a => a.GenreId.Equals(id1))
                .ToList();
        }

    }
}
 