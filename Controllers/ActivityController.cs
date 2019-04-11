using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Activity.Models;

namespace Activity.Controllers
{
    [Route("Activities")]
    public class ActivityController : Controller
    {
        private ActivityUser loggedInUser
        {
            get { return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId")); }
        } 
        private ActivityContext dbContext;
        public ActivityController(ActivityContext context)
        {
            dbContext = context;
        }
        // localhost:5000/Activities
        [HttpGet("")]
        public IActionResult Index()
        {
            // if no key in session, redirect back to Home->Index
            if(loggedInUser == null)
                return RedirectToAction("Index", "Home");
            ViewBag.UserId = loggedInUser.UserId;

            var allActivities = dbContext.Activities
                .Include(w => w.Responses)
                .OrderByDescending(w => w.Date)
                .ToList();
            
            ViewBag.AllActivities = allActivities;

            return View(allActivities);
        }

        [HttpGet("{ActivityId}")]
        public IActionResult Show(int ActivityId)
        {
            // query for Activity with Activity id
            // include Responses
            // THEN include R.Guest
            var viewModel = dbContext.Activities
                .FirstOrDefault(w => w.ActivityId == ActivityId);

            return View(viewModel);
        }

        [HttpGet("/rsvp/{ActivityId}")]
        public IActionResult Rsvp(int ActivityId)
        {
            // is user actually in session?
            if(loggedInUser == null)
                return RedirectToAction("Index");

            Response rsvp = new Response()
            {
                UserId = loggedInUser.UserId,
                ActivityId = ActivityId
            };

            dbContext.Responses.Add(rsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("/unrsvp/{ActivityId}")]
        public IActionResult Unrsvp(int ActivityId)
        {
            // is user actually in session?
            if(loggedInUser == null)
                return RedirectToAction("Index");

            // query for response to delete
            Response toRemove = dbContext.Responses.FirstOrDefault(
                r => r.ActivityId == ActivityId &&
                     r.UserId == loggedInUser.UserId
            );

            if(toRemove == null)
                return RedirectToAction("Index");

            dbContext.Responses.Remove(toRemove);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}