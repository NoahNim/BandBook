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
  }
}
