using CodingEvents.Data;
using CodingEvents.Models;
using CodingEvents.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingEvents.Controllers
{
    public class EventsController : Controller
    {
        private EventDbContext _context;

        public EventsController(EventDbContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index()
        {
            List<Event> events = _context.Events
                .Include(e => e.Category)
                .ToList();

            return View(events);
        }

        public IActionResult Add()
        {
            List<EventCategory> categories = _context.EventsCategory.ToList();
            AddEventViewModel addEventViewModel = new AddEventViewModel(categories);

            return View(addEventViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddEventViewModel addEventViewModel)
        {
            if (ModelState.IsValid)
            {
                EventCategory theCategory = _context.EventsCategory.Find(addEventViewModel.CategoryId);
                Event newEvent = new Event
                {
                    Name = addEventViewModel.Name,
                    Description = addEventViewModel.Description,
                    ContactEmail = addEventViewModel.ContactEmail,
                    Location = addEventViewModel.Location,
                    NumberOfAttendees = addEventViewModel.NumberOfAttendees,
                    Category = theCategory
                };

                _context.Events.Add(newEvent); // This stages the data
                _context.SaveChanges(); // This actually saves the data in the DB

                return Redirect("/Events");
            }

            return View(addEventViewModel);
        }

        public IActionResult Delete()
        {
            ViewBag.events = _context.Events.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int[] eventIds)
        {
            foreach (int eventId in eventIds)
            {
                Event theEvent = _context.Events.Find(eventId);
                _context.Events.Remove(theEvent);
            }

            _context.SaveChanges();

            return Redirect("/Events");
        }

        // /Events/Detail/5 
        public IActionResult Detail(int id)
        {
            Event theEvent = _context.Events
                .Include(e => e.Category)
                .Single(e => e.Id == id);

            List<EventTag> eventTags = _context.EventTags
                .Where(et => et.EventId == id)
                .Include(et => et.Tag)
                .ToList();


            EventDetailViewModel viewModel = new EventDetailViewModel(theEvent, eventTags);

            return View(viewModel);
        }


        //[Route("/Events/Edit/{eventId}")] //Question 3
        //public IActionResult Edit(int eventId) //Question 1.a
        //{
        //    _context.Events.GetById(eventId); //Question 6.a
        //    ViewBag.eventToEdit = editEvent; //Question 6.b
        //    ViewBag.title = $"Edit Event {editEvent.Name} {editEvent.Id}"; //Question 9
        //    return View(); //Question 6.c
        //}

        //[HttpPost]
        //[Route("/Events/Edit")]  //Question 2 
        //public IActionResult SubmitEditEventForm(int eventId, string name, string description) //Question 1.b
        //{
        //    Event eventToEdit = EventData.GetById(eventId); //Question 10.a
        //    eventToEdit.Name = name; //Question 10.b
        //    eventToEdit.Description = description; //Question 10.b
        //    return Redirect("/Events"); //Question 10.c
        //}
    }
}
