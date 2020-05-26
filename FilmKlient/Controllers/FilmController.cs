using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using FilmKlient.Models;

namespace FilmKlient.Controllers
{
    public class FilmController : Controller
    {
        // GET: Film
        public ActionResult Index()
        {
            IEnumerable<Film> Filmer = null;
            using(var Klient = new HttpClient())
            {
                Klient.BaseAddress = new Uri("http://193.10.202.71/Filmservice/");
                var SvarUppgift = Klient.GetAsync("Film");
                SvarUppgift.Wait();
                var Resultat = SvarUppgift.Result;
           
                if (Resultat.IsSuccessStatusCode)
                {
                    var LasUppgift = Resultat.Content.ReadAsAsync<IList<Film>>();
                    LasUppgift.Wait();

                    Filmer = LasUppgift.Result;
                }
                else
                {
                    Filmer = Enumerable.Empty<Film>();
                    ModelState.AddModelError(string.Empty, "Server is currently down please try later");
                    //Retunera felet här
                }
            }
            return View(Filmer);
        }
        
        //Post
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Film film)
        {
            using(var Klient = new HttpClient())
            {
                var path = "http://193.10.202.71/filmadmin/Images/" + film.Filmbild;//publicera film admin gränssnittet 
                film.Filmbild = path;

                //spara filmen i film mappen image
                var fileName = Path.GetFileName(film.File.FileName);
                var paths = Path.Combine(Server.MapPath("~/Images"), fileName);
                film.File.SaveAs(paths);

                film.File = null;



                Klient.BaseAddress = new Uri("http://193.10.202.71/Filmservice/film");
                var SkickaUppgift = Klient.PostAsJsonAsync<Film>("Film", film);


                SkickaUppgift.Wait();

                var SkickatResultat = SkickaUppgift.Result;

                if(SkickatResultat.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");

                }
                ModelState.AddModelError(string.Empty, "Server is currently down please try later, please check with admin");
            }
            return View(film);
            
        }

        public ActionResult Edit (int id)
        {
            Film film = null;
            using(var Klient = new HttpClient())
            {
                Klient.BaseAddress = new Uri("http://193.10.202.71/Filmservice/film");
                var SvarUppgift = Klient.GetAsync("Film/" + id.ToString());
                SvarUppgift.Wait();
                var Resultat = SvarUppgift.Result;

                if(Resultat.IsSuccessStatusCode) {

                    var LasUppgift = Resultat.Content.ReadAsAsync<Film>();
                    LasUppgift.Wait();
                    film = LasUppgift.Result;
                }  
            }
            return View(film);
        }

        //Create a post method to update the data
        [HttpPost]
        public ActionResult Edit(Film film)
        {
            using (var Klient = new HttpClient())
            {
                Klient.BaseAddress = new Uri("http://193.10.202.71/Filmservice/film");
                var AndraUppgift = Klient.PutAsJsonAsync<Film>("film", film);
                AndraUppgift.Wait();

                var Resultat = AndraUppgift.Result;
                if (Resultat.IsSuccessStatusCode)
                return View(film);
                return RedirectToAction("Index");
                  

                
            }
            

        }

        public ActionResult Delete(int? id)
        {
            Film film = null;
            using (var Klient = new HttpClient())
            {
                Klient.BaseAddress = new Uri("http://193.10.202.71/Filmservice/film");
                var RaderaUppgift = Klient.GetAsync("Film/" + id.ToString());
                RaderaUppgift.Wait();

                var Resultat = RaderaUppgift.Result;
                if (Resultat.IsSuccessStatusCode)
                {
                    var lasUppgift = Resultat.Content.ReadAsAsync<Film>();
                    lasUppgift.Wait();
                    film = lasUppgift.Result;
                }
                 return View(film);
            }
           
        }
            //Delete action
            [HttpPost]
            public ActionResult Delete(int id)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://193.10.202.71/Filmservice/");

                    //HTTP DELETE
                    var deleteTask = client.DeleteAsync("film/" + id.ToString());
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
        }
    }
