using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class VenueController : Controller
    {

        [HttpGet("/venues/index")]
        public ActionResult Index()
        {
            List<Venue> allVenues = Venue.GetAll();
            return View(allVenues);
        }
        [HttpGet("/venues/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/venues")]
        public ActionResult Create()
        {
            Venue newVenue = new Venue(Request.Form["venue-name"], Request.Form["venue-location"]);
            newVenue.Save();
            return RedirectToAction("Success", "Home");
        }
        [HttpGet("/venues/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Venue selectedVenue = Venue.Find(id);
            List<Band> venueBands = selectedVenue.GetBands();
            List<Band> allBands = Band.GetAll();
            model.Add("selectedVenue", selectedVenue);
            model.Add("venueBands", venueBands);
            model.Add("allBands", allBands);
            return View(model);
        }
        [HttpPost("/venues/{venueId}/bands/new")]
        public ActionResult AddBand(int venueId)
        {
            Venue venue = Venue.Find(venueId);
            Band band = Band.Find(Int32.Parse(Request.Form["band-id"]));
            venue.AddBand(band);
            return RedirectToAction("Details",  new { id = venueId });
        }
        [HttpGet("/venues/{venueId}/delete")]
        public ActionResult DeleteVenue(int venueId)
        {
          Venue venue = Venue.Find(venueId);
          venue.Delete();
          List<Venue> allVenues = Venue.GetAll();
          return View("Index", allVenues);
        }
        [HttpGet("/venues/deleteall")]
        public ActionResult DeleteAll()
        {
            Venue.DeleteAll();
            List<Venue> allVenues = Venue.GetAll();
            return View("Index", allVenues);
        }
    }
}
