using CodingEvents.Data;
using CodingEvents.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingEvents.Controllers
{
    public class EventsController : Controller
    {

        public IActionResult Index()
        {
            ViewBag.events = EventData.GetAll();

            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("/Events/Add")]
        public IActionResult NewEvent(Event newEvent)
        {
            EventData.Add(newEvent);

            return Redirect("/Events");
        }

        public IActionResult Delete()
        {
            ViewBag.events = EventData.GetAll();

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int[] eventIds)
        {
            foreach (int eventId in eventIds)
            {
                EventData.Remove(eventId);
            }

            return Redirect("/Events");
        }

        [Route("/Events/Edit/{eventId}")] //Question 3
        public IActionResult Edit(int eventId) //Question 1.a
        {
            Event editEvent = EventData.GetById(eventId); //Question 6.a
            ViewBag.eventToEdit = editEvent; //Question 6.b
            ViewBag.title = $"Edit Event {editEvent.Name} {editEvent.Id}"; //Question 9
            return View(); //Question 6.c
        }

        [HttpPost]
        [Route("/Events/Edit")]  //Question 2 
        public IActionResult SubmitEditEventForm(int eventId, string name, string description) //Question 1.b
        {
            Event eventToEdit = EventData.GetById(eventId); //Question 10.a
            eventToEdit.Name = name; //Question 10.b
            eventToEdit.Description = description; //Question 10.b
            return Redirect("/Events"); //Question 10.c
        }
    }
}
