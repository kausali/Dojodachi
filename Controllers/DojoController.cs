using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

 
namespace dojodachi.Controllers
{
    public static class SessionExtensions
        {
            public static void SetObjectAsJson(this ISession session, string key, object value)
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }
            public static T GetObjectFromJson<T>(this ISession session, string key)
            {
                string value = session.GetString(key);
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }
        }

        public class DojoController : Controller
       {
            [HttpGet]
            [Route("")]
            public IActionResult Index()
            {
                // "dachi" - key ,could name anything!
                // var status = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                if (HttpContext.Session.GetObjectFromJson<dachi>("dachi") == null)
                {
                    var status = new dachi();
                    HttpContext.Session.SetObjectAsJson("dachi", status);
                    TempData["message"] = "Click the buttons below to play the game!";
                    // ViewBag.Message = TempData["message"];
                } else {
                    var status = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                
                    if (status.Happiness <= 0)
                    {
                        ViewBag.Win = false;
                        TempData["message"] = "your dog is out of happiness! GAME OVER:(";
                        // return View("Index");
                    }
                    if (status.Fullness <= 0)
                    {
                        ViewBag.Win = false;
                        TempData["message"] = "Your dog is out of Fullness! GAME OVER:(";
                        // return View("Index");
                    }
                    
                    if (status.Fullness >= 100 && status.Happiness >=100 && status.Energy >=100)
                    {
                        ViewBag.Win= true;
                        TempData["message"] = "You Won!!";
                        // return View("Index");
                    }
                }
                ViewBag.Message = TempData["message"];
                ViewBag.MyDog = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                return View("Index");
              
                
            }
            [HttpPost]
            [Route("feed")]
            public IActionResult Feed()
            {
                Random Rand = new Random();
                var status = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                if (status.Meals > 0)
                {
                status.Meals -= 1;
                int randFullness = Rand.Next(5, 11);
                status.Fullness += randFullness;
                TempData["message"] = "You fed your Dojodachi. Fullness +" + randFullness +" Meal -1";
                ViewBag.Message = TempData["message"];
                HttpContext.Session.SetObjectAsJson("dachi", status);
                return RedirectToAction("Index");
                }
                TempData["message"] = "Sorry you are out of Meal :(";
                return RedirectToAction("Index");
                
            }
            [HttpPost]
            [Route("play")]
            public IActionResult Play()
            {
                Random Rand = new Random();
                var status = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                if (status.Energy <= 0)
                {
                    TempData["message"] = "Sorry you are out of Energy :(";
                    return RedirectToAction("Index");
                }
                status.Energy -= 5;
                int randHappiness = Rand.Next(5, 10);
                status.Happiness += randHappiness;
                TempData["message"] = "You fed your Dojodachi. Fullness +" + randHappiness +" Energy -5";
                ViewBag.Message = TempData["message"];
                HttpContext.Session.SetObjectAsJson("dachi", status);
                return RedirectToAction("Index");
            }
            [HttpPost]
            [Route("sleep")]
            public IActionResult Sleep()
            {
                
                var status = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                if (status.Fullness <= 0)
                {
                    TempData["message"] = "Sorry your dog is out of fullness :(";
                    return RedirectToAction("Index");
                }
                status.Energy += 15;
                status.Fullness -= 5;
                status.Happiness -=5;
                TempData["message"] = "Your Dojodachi is sleeping now. Energy +15,  Fullness -5 Happiness -5" ;
                ViewBag.Message = TempData["message"];
                HttpContext.Session.SetObjectAsJson("dachi", status);
                return RedirectToAction("Index");
            }
            [HttpPost]
            [Route("work")]
            public IActionResult Work()
            {
              Random Rand = new Random();
                var status = HttpContext.Session.GetObjectFromJson<dachi>("dachi");
                if (status.Energy <= 0)
                {
                    TempData["message"] = "Sorry you are out of Energy :(";
                    return RedirectToAction("Index");
                }
                status.Energy -= 5;
                int randMeals = Rand.Next(1, 3);
                status.Meals += randMeals;
                TempData["message"] = "You Worked. Meals+ " + randMeals +" Energy -5";
                HttpContext.Session.SetObjectAsJson("dachi", status);
                return RedirectToAction("Index");
            }
            [HttpPost]
            [Route("reset")]
            public IActionResult Reset()
            {
                HttpContext.Session.Clear();
                // dachi newDachi = new dachi();
                // HttpContext.Session.SetObjectAsJson("dachi", newDachi);
                return RedirectToAction("Index");
                // return View("Index");

            }
        }  
}