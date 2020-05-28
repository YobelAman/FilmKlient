using FilmKlient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using NLog;
namespace FilmKlient.Controllers
{
    public class SalongController : Controller
    {
        // GET: Salong
        public ActionResult Index()
        {

            IEnumerable<Salong> Salonger = null;
            using (var Klient = new HttpClient())
            {
                try
                {


                    Klient.BaseAddress = new Uri("https://localhost:44379/");
                    var SvarUppgift = Klient.GetAsync("Salong");
                    SvarUppgift.Wait();
                    var Resultat = SvarUppgift.Result;

                    if (Resultat.IsSuccessStatusCode)
                    {
                        var LasUppgift = Resultat.Content.ReadAsAsync<IList<Salong>>();
                        LasUppgift.Wait();

                        Salonger = LasUppgift.Result;
                    }
                    else
                    {
                        Salonger = Enumerable.Empty<Salong>();
                        ModelState.AddModelError(string.Empty, "Server is currently down please try later");
                        //Retunera felet här
                    }
                }
                catch (DivideByZeroException ex)
                {

                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error("ops!", ex);
                }
            }
            return View(Salonger);
        }

        //Post
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Salong salong)
        {
            using (var Klient = new HttpClient())
            {
                try
                {


                    Klient.BaseAddress = new Uri("https://localhost:44379/film");
                    var SkickaUppgift = Klient.PostAsJsonAsync<Salong>("Salong", salong);
                    SkickaUppgift.Wait();

                    var SkickatResultat = SkickaUppgift.Result;

                    if (SkickatResultat.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");

                    }
                    ModelState.AddModelError(string.Empty, "Server is currently down please try later, please check with admin");
                }
                catch (DivideByZeroException ex)
                {

                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error("ops!", ex);
                }
            }
            return View(salong);

        }

        public ActionResult Edit(int id)
        {
            Salong salong = null;
            using (var Klient = new HttpClient())
            {
                try
                {


                    Klient.BaseAddress = new Uri("https://localhost:44379/film");
                    var SvarUppgift = Klient.GetAsync("Salong/" + id.ToString());
                    SvarUppgift.Wait();
                    var Resultat = SvarUppgift.Result;

                    if (Resultat.IsSuccessStatusCode)
                    {

                        var LasUppgift = Resultat.Content.ReadAsAsync<Salong>();
                        LasUppgift.Wait();
                        salong = LasUppgift.Result;
                    }
                }
                catch (DivideByZeroException ex)
                {

                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error("ops!", ex);
                }
            }
            return View(salong);
        }

        //Create a post method to update the data
        [HttpPost]
        public ActionResult Edit(Salong salong)
        {
            using (var Klient = new HttpClient())
            {
                try
                {

                    Klient.BaseAddress = new Uri("https://localhost:44379/Film");
                    var AndraUppgift = Klient.PutAsJsonAsync<Salong>("salong", salong);
                    AndraUppgift.Wait();

                    var Resultat = AndraUppgift.Result;
                    if (Resultat.IsSuccessStatusCode)
                        return View(salong);

                }
                catch (DivideByZeroException ex)
                {

                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error("ops!", ex);
                }
                return RedirectToAction("Index");

            }


        }

        public ActionResult Delete(int? id)
        {
            Salong salong = null;
            using (var Klient = new HttpClient())
            {
                try
                {


                    Klient.BaseAddress = new Uri("https://localhost:44379/");
                    var RaderaUppgift = Klient.GetAsync("Salong/" + id.ToString());
                    RaderaUppgift.Wait();

                    var Resultat = RaderaUppgift.Result;
                    if (Resultat.IsSuccessStatusCode)
                    {
                        var lasUppgift = Resultat.Content.ReadAsAsync<Salong>();
                        lasUppgift.Wait();
                        salong = lasUppgift.Result;
                    }
                }
                catch (DivideByZeroException ex)
                {

                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error("ops!", ex);
                }
                return View(salong);
            }

        }
        //Delete action
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {


                    client.BaseAddress = new Uri("https://localhost:44379/");

                    //HTTP DELETE
                    var deleteTask = client.DeleteAsync("salong/" + id.ToString());
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Index");
                    }
                }
                catch (DivideByZeroException ex)
                {

                    Logger logger = LogManager.GetLogger("fileLogger");
                    logger.Error("ops!", ex);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
