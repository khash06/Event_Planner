using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Belt.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
    public IActionResult Register(User user)
    {
        var EmailCheck = _context.users.SingleOrDefault(x => x.Email == user.Email);
        if(ModelState.IsValid && EmailCheck == null)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            
            
            // HttpContext.Session.SetString("Name", EmailCheck.FirstName);
            return RedirectToAction("AddData", user);
        }
        else
        {
            ViewBag.message = "Email already in database";
            return View("Index");
        }
    }
[HttpPost]
    public IActionResult Login(string Email, string Password, string FirstName)
    {
        
        var EmailUser = _context.users.SingleOrDefault(x => x.Email == Email);
        if(EmailUser != null && Password != null)
        {
            var Hasher = new PasswordHasher<User_Reg>();
            if(0 != Hasher.VerifyHashedPassword(EmailUser, EmailUser.Password, Password))
            {
                HttpContext.Session.SetInt32("id", EmailUser.UserId);
                HttpContext.Session.SetString("Name", EmailUser.FirstName);
                return RedirectToAction("DashboardPage");
            }
            else
            {
                return View("Index");
            }
        }
        ViewBag.login = "Email or password incorrect";
        return View("Index");
    }

    public IActionResult AddData(User_Reg Reg)
    {
            _context.Add(Reg);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("id", Reg.UserId);
            return RedirectToAction("DashboardPage");
    }

    [Route("DashboardPage")]
    public IActionResult DashboardPage(int ActivitiesId)
    {
        int? UserID = HttpContext.Session.GetInt32("id");
        ViewBag.UserID = UserID;

        if(HttpContext.Session.GetInt32("id") == null)
        {
            return RedirectToAction("Index");
        }

        var userinfo = _context.users.SingleOrDefault(a => a.UserId == UserID);
        string FirstName = userinfo.FirstName;
        ViewBag.name = userinfo.FirstName;

        var guest = _context.activities.Include(r => r.Reserver).ThenInclude(u => u.User_Reg).ToList();
        var titles = guest.OrderByDescending(l => l.Date);
        ViewBag.guest = guest;
        ViewBag.titles = titles;

        var stuff = _context.attendess.Include(p => p.User_Reg).SingleOrDefault(o => o.ActivitiesId == ActivitiesId);
        ViewBag.stuff = stuff;

        Activities names = _context.activities.SingleOrDefault(a => a.ActivitiesId == ActivitiesId);
        ViewBag.names = names;

        var coord = _context.activities.Include(i => i.Reserver).ThenInclude(p => p.User_Reg).Where(m =>m.Creator == ActivitiesId);
        ViewBag.coord = coord;

        

        // var weddings = _context.weddings.ToList();
        // ViewBag.weddings = weddings;

        // var joined = _context.activities.Include(t => t.Reserver).ThenInclude(h => h.User_Reg).ToList();
        // ViewBag.joined = joined;
        
        return View();
    }

    [Route("AddActivity")]
    public IActionResult AddActivity()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateActivity(Activities activities)
    {
        int? UserID = HttpContext.Session.GetInt32("id");
        
            if(ModelState.IsValid){
            var newactivity = new Activities()
            {
                Title = activities.Title,
                StartTime = activities.StartTime,
                Date = activities.Date,
                Duration = activities.Duration,
                HrMin = activities.HrMin,
                Descripton = activities.Descripton
            };

            newactivity.Creator = (int) UserID;
            newactivity.Participants++;
            _context.Add(newactivity);
            _context.SaveChanges();

            var newattendee = new Attendee();

            newattendee.UserId = (int) UserID;
            newattendee.ActivitiesId = newactivity.ActivitiesId;
            
            _context.Add(newattendee);
            _context.SaveChanges();


            return Redirect($"ActivitiesPage/{newactivity.ActivitiesId}");
        }
        else 
        {
            return View("AddActivity");
        }
    }

    [HttpGet]
    [Route("Home/ActivitiesPage/{ActivitiesId}")]

    public IActionResult ActivitiesPage(Activities activities, Attendee attendee, int ActivitiesId, int UserId)
    {
        int? UserID = HttpContext.Session.GetInt32("id");

        Activities title = _context.activities.SingleOrDefault(a => a.ActivitiesId == ActivitiesId);
        var joined = _context.activities.Include(r => r.Reserver).ThenInclude(u => u.User_Reg).SingleOrDefault(a => a.ActivitiesId == ActivitiesId);
        ViewBag.joined = joined;
        ViewBag.title = title;
        //var host = _context.activities.Include(c => c.Reserver).SingleOrDefault(k => k.ActivitiesId == ActivitiesId);
        //ViewBag.host = host;
        var host = _context.activities.Include(r => r.Reserver).ThenInclude(u => u.User_Reg).ToList().FirstOrDefault();
        ViewBag.host = host;

        var guests = _context.activities.Include(r => r.Reserver).ThenInclude(u => u.User_Reg).ToList();
        ViewBag.guests = guests;

        // var guests = _context.attendess.SingleOrDefault(d => d.ActivitiesId == ActivitiesId && d.UserId == UserID);
        // ViewBag.guests = guests;
        


        return View();
    }

    [Route("/Delete/{ActivitiesId}")]
    public IActionResult DeleteActivity(int ActivitiesId)
    {
        var delactivity = _context.activities.SingleOrDefault(w => w.ActivitiesId == ActivitiesId);
        _context.activities.Remove(delactivity);
        _context.SaveChanges();
        return RedirectToAction("DashboardPage");
    }

    [Route("/leave/{ActivitiesId}")]
    public IActionResult leave(int ActivitiesId)
    {
        int? UserID = HttpContext.Session.GetInt32("id");
        var reduce = _context.activities.SingleOrDefault(d => d.ActivitiesId == ActivitiesId);
        reduce.Participants--;
        var remove = _context.attendess.SingleOrDefault(d => d.ActivitiesId == ActivitiesId && d.UserId == UserID);
        _context.attendess.Remove(remove);
        _context.SaveChanges();
        return RedirectToAction("DashboardPage");
    }

    [Route("/join/{ActivitiesId}")]
    public IActionResult join(int ActivitiesId)
    {
        int? UserID = HttpContext.Session.GetInt32("id");
        var increase = _context.activities.SingleOrDefault(i => i.ActivitiesId == ActivitiesId);
        increase.Participants++;
        Attendee join = new Attendee();
        join.UserId = (int)UserID;
        join.ActivitiesId = ActivitiesId;
        _context.Add(join);
        _context.SaveChanges();
        return RedirectToAction("DashboardPage");
    }

    [Route("LogOff")]
    public IActionResult LogOff()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private BeltContext _context;
        
        public HomeController(BeltContext context)
        {
            _context = context;
        }
    }
}
