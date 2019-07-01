using MoviesProject.Database;
using MoviesProject.Filters;
using MoviesProject.Managers;
using MoviesProject.Messages;
using MoviesProject.Models;
using MoviesProject.Repository;
using MoviesProject.Results;
using MoviesProject.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MoviesProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
           
            MovieDBEntities1 db = new MovieDBEntities1();
            var list = db.Movies.ToList();
            return View(list);
            
        }
        public ActionResult Detay(int id)
        {
            MovieDBEntities1 db = new MovieDBEntities1();
            var list = db.Movies.ToList();
            DetayModel detayModel = new DetayModel();
            Movies movie = list.Find(x => x.Id == id);
            detayModel.movie = movie;

            detayModel.comments = detayModel.movie.Comments.OrderByDescending(x => x.Id).ToList();
            return View(detayModel);
        }
        [HttpPost]
        public ActionResult Detay(DetayModel detayModel)
        {
            Repository<Movies> movies = new Repository<Movies>();
            Repository<Comments> comments = new Repository<Comments>();
            Comments comment = new Comments();
            var list = movies.List();
            comment.OwnerId = CurrentSession.User.Id;
            comment.Text = detayModel.eklenen_Yorum;
            comment.MovieId = detayModel.movieId;
            comments.Insert(comment);
            return RedirectToAction("Detay/"+detayModel.movieId);
        }
        public ActionResult Category(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = new MovieDBEntities1();
            var list = db.Categories.ToList();
            Categories cat = list.Find(x => x.Id == id);
            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Movies.OrderByDescending(x => x.ReleaseDate).ToList());
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            UserManager um = new UserManager();
            Result<User> res = um.LoginUser(model);

            if (ModelState.IsValid)
            {
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));



                    return View(model);
                }

                CurrentSession.Set("login", res.Resultt);
                return RedirectToAction("Index");

            }

            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager eum = new UserManager();

                Result<User> res = eum.RegisterUser(model);

                if (res.Errors.Count > 0)
                {

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }              
             
                return RedirectToAction("RegisterOk");
            }
            return View(model);
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
        [AuthAdmin]
        public ActionResult UserPanel()
        {
            Repository<User> users = new Repository<User>();
            List<User> userlist = new List<User>();
            userlist = users.List();
            return View(userlist);
        }
        [AuthAdmin]
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repository<User> users = new Repository<User>();

            User User = users.Find(x => x.Id == id.Value);

            if (User == null)
            {
                return HttpNotFound();
            }

            return View(User);
        }
        [AuthAdmin]
        [HttpPost]
        public ActionResult EditUser(User User)
        {
            Repository<User> users = new Repository<User>();

            if (ModelState.IsValid)
            {
                
                int isUpdate = users.Update(User);
                if(isUpdate == 0)
                {
                    Result<User> res = new Result<User>();
                    res.AddError(ErrorMessageCode.UpdateFailed, "Güncelleme Başarısız");
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(User);
                }
               
              

                return RedirectToAction("UserPanel");
            }
            return View(User);
        }
        [AuthAdmin]
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Repository<User> users = new Repository<User>();

            User User = users.Find(x => x.Id == id.Value);

            if (User == null)
            {
                return HttpNotFound();
            }

            return View(User);
        }
        [HttpPost]
        [AuthAdmin]
        public ActionResult DeleteUser(int id)
        {
            Repository<User> users = new Repository<User>();
            User user = users.Find(x => x.Id == id);
            Repository<Comments> comments = new Repository<Comments>();
            Comments comment = new Comments();
            comment = comments.Find(x => x.OwnerId == id);
            while (comment != null)
            {
                comments.Delete(comment);
                comment =  comments.Find(x => x.OwnerId == id);
              
            }
            users.Delete(user);

            return RedirectToAction("UserPanel");
        }
        [AuthAdmin]
        public ActionResult CreateUser()
        {
            return View();
        }
        [AuthAdmin]
        [HttpPost]
        public ActionResult CreateUser(User User)
        {
           

            if (ModelState.IsValid)
            {
                Repository<User> users = new Repository<User>();
                int i = users.Insert(User);
                Result<User> res = new Result<User>();

                if (i == 0)
                {
                    res.AddError(ErrorMessageCode.UserCreateFailed, "Kullanıcı Oluşturulamadı!");
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(User);
                }

                return RedirectToAction("UserPanel");
            }

            return View(User);
        }
        [AuthAdmin]
        public ActionResult MoviePanel()
        {
            Repository<Movies> movies = new Repository<Movies>();
            List<Movies> movielist = new List<Movies>();
            movielist = movies.List();
            return View(movielist);
        }
        [AuthAdmin]
        public ActionResult EditMovie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repository<Movies> movies = new Repository<Movies>();

            Movies movie = movies.Find(x => x.Id == id.Value);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }
        [AuthAdmin]
        [HttpPost]
        public ActionResult EditMovie(Movies movie)
        {
            Repository<Movies> movies = new Repository<Movies>();

            if (ModelState.IsValid)
            {

                int isUpdate = movies.Update(movie);
                if (isUpdate == 0)
                {
                    Result<Movies> res = new Result<Movies>();
                    res.AddError(ErrorMessageCode.UpdateFailed, "Güncelleme Başarısız");
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(movie);
                }



                return RedirectToAction("MoviePanel");
            }
            return View(movie);
        }
        [AuthAdmin]
        public ActionResult DeleteMovie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Repository<Movies> movies = new Repository<Movies>();

            Movies movie = movies.Find(x => x.Id == id.Value);

            if (User == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }
        [AuthAdmin]
        [HttpPost]

        public ActionResult DeleteMovie(int id)
        {
            Repository<Movies> movies = new Repository<Movies>();
            Movies movie = movies.Find(x => x.Id == id);
            Repository<Comments> comments = new Repository<Comments>();
            Comments comment = new Comments();
            comment = comments.Find(x => x.MovieId == id);
            while (comment != null)
            {
                comments.Delete(comment);
                comment = comments.Find(x => x.MovieId == id);

            }
            movies.Delete(movie);

            return RedirectToAction("MoviePanel");
        }
        [AuthAdmin]
        public ActionResult CreateMovies()
        {
            return View();
        }
        [AuthAdmin]
        [HttpPost]
        public ActionResult CreateMovies(Movies movie)
        {


            if (ModelState.IsValid)
            {
                Repository<Movies> movies = new Repository<Movies>();
                int i = movies.Insert(movie);
                Result<Movies> res = new Result<Movies>();

                if (i == 0)
                {
                    res.AddError(ErrorMessageCode.UserCreateFailed, "Kullanıcı Oluşturulamadı!");
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(User);
                }

                return RedirectToAction("MoviePanel");
            }

            return View(User);
        }

        public ActionResult Logout()
        {
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult ChangeLang(String LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }
    }
}