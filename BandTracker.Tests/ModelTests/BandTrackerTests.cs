using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;
using BandTracker;
using MySql.Data.MySqlClient;

namespace BandTracker.Tests
{
  [TestClass]
  public class BandTrackerTest : IDisposable
  {
    public BandTrackerTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
    }
    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }
    [TestMethod]
    public void Save_VenueSavesToDatabase_Bands()
    {
      //Arrange
      Venue testVenue = new Venue("Meowcity", "Seattle");
      testVenue.Save();

      //Act
      List<Venue> venueResult = Venue.GetAll();
      List<Venue> venueList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(venueList, venueResult);
    }
    [TestMethod]
    public void Save_BandSavesToDatabase_Bands()
    {
      //Arrange
      Band testBand = new Band("Meow");
      testBand.Save();

      //Act
      List<Band> bandResult = Band.GetAll();
      List<Band> bandList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(bandList, bandResult);
    }
    [TestMethod]
    public void AddVenue_AddsVenueToBand_VenueList()
    {
      //Arrange
      Band testBand = new Band("Rush");
      testBand.Save();

      Venue testVenue = new Venue("Seattle Place", "Seattle");
      testVenue.Save();

      //Act
      testBand.AddVenue(testVenue);

      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetVenues_ReturnsAllBandVenues_VenueList()
    {
      //Arrange
      Band testBand = new Band("Cat");
      testBand.Save();

      Venue testVenue1 = new Venue("Musicland", "City");
      testVenue1.Save();

      Venue testVenue2 = new Venue("Hawaii Hall", "LA");
      testVenue2.Save();

      //Act
      testBand.AddVenue(testVenue1);
      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue> {testVenue1, testVenue2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void AddBand_AddsBandToVenue_BandList()
    {
      //Arrange
      Band testBand = new Band("Rush");
      testBand.Save();

      Venue testVenue = new Venue("Seattle Place", "Seattle");
      testVenue.Save();

      //Act
      testVenue.AddBand(testBand);

      List<Band> result = testVenue.GetBands();
      List<Band> testList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetBands_ReturnsAllBandBands_BandList()
    {
      //Arrange
      Venue testVenue = new Venue("Cat Music", "Cat Land");
      testVenue.Save();

      Band testBand1 = new Band("Music");
      testBand1.Save();

      Band testBand2 = new Band("This Is A Band");
      testBand2.Save();

      //Act
      testVenue.AddBand(testBand1);
      List<Band> result = testVenue.GetBands();
      List<Band> testList = new List<Band> {testBand1, testBand2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Delete_DeletesVenueInDataBase_Venuess()
    {
      Venue testVenue = new Venue("Meowland", "Catplace");
      testVenue.Save();
      Venue otherVenue = new Venue("Science Center", "Seattle");
      otherVenue.Save();

      testVenue.Delete();
      List<Venue> result = Venue.GetAll();
      List<Venue> expected = new List<Venue>{otherVenue};

      CollectionAssert.AreEqual(expected, result);
    }
    [TestMethod]
    public void TestUpdateVenue()
    {
      Venue newVenue = new Venue("Seattle City Music Place", "Seattle");
      newVenue.Save();

      newVenue.UpdateVenue("Seattle Paramount Place", "Renton");
      Venue foundVenue = Venue.Find(newVenue.GetId());

      // Console.WriteLine("Venue Title: " + found.GetVenueTitle());
      Assert.AreEqual("Seattle Paramount Place", foundVenue.GetName());

    }
  }
}
