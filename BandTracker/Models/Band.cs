using System;
using MySql.Data.MySqlClient;
using BandTracker;
using System.Collections.Generic;

namespace BandTracker.Models
{
  public class Band
  {
    private int _band_id;
    private string _name;

    public Band(string name, int band_id = 0)
    {
      _band_id = band_id;
      _name = name;
    }
    public string GetName()
    {
        return _name;
    }
    public int GetId()
    {
        return _band_id;
    }
    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
         Band newBand = (Band) otherBand;
         bool idEquality = this.GetId() == newBand.GetId();
         bool nameEquality = this.GetName() == newBand.GetName();
         return (idEquality && nameEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetName().GetHashCode();
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO bands (name) VALUES (@Name);";

        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@Name";
        name.Value = this._name;
        cmd.Parameters.Add(name);

        cmd.ExecuteNonQuery();
        _band_id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public static List<Band> GetAll()
    {
         List<Band> allBands = new List<Band> {};
         MySqlConnection conn = DB.Connection();
         conn.Open();
         var cmd = conn.CreateCommand() as MySqlCommand;
         cmd.CommandText = @"SELECT * FROM bands;";
         var rdr = cmd.ExecuteReader() as MySqlDataReader;
         while(rdr.Read())
         {
           int bandId = rdr.GetInt32(0);
           string bandName = rdr.GetString(1);

           Band newBand = new Band(bandName, bandId);
           allBands.Add(newBand);
         }
         conn.Close();
         if (conn != null)
         {
             conn.Dispose();
         }
         return allBands;
    }
    public void AddVenue(Venue newVenue)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO tracker_info (band_id, venue_id) VALUES (@bandId, @venueId);";

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@bandId";
        band_id.Value = _band_id;
        cmd.Parameters.Add(band_id);

        MySqlParameter venue_id = new MySqlParameter();
        venue_id.ParameterName = "@venueId";
        venue_id.Value = newVenue.GetId();
        cmd.Parameters.Add(venue_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public List<Venue> GetVenues()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT venues.* FROM bands
        JOIN tracker_info ON (band_id = tracker_info.band_id)
        JOIN venues ON(tracker_info.venue_id = venue_id)
        WHERE band_id = @bandId";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@bandId";
        bandIdParameter.Value = _band_id;
        cmd.Parameters.Add(bandIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> venueIds = new List<int> {};
        while(rdr.Read())
        {
            int venueId = rdr.GetInt32(0);
            venueIds.Add(venueId);
        }
        rdr.Dispose();

        List<Venue> venues = new List<Venue> {};
        foreach (int venueId in venueIds)
        {
            var venueQuery = conn.CreateCommand() as MySqlCommand;
            venueQuery.CommandText = @"SELECT * FROM venues WHERE id = @VenueId;";

            MySqlParameter venueIdParameter = new MySqlParameter();
            venueIdParameter.ParameterName = "@VenueId";
            venueIdParameter.Value = venueId;
            venueQuery.Parameters.Add(venueIdParameter);

            var venueQueryRdr = venueQuery.ExecuteReader() as MySqlDataReader;
            while(venueQueryRdr.Read())
            {
                int thisVenueId = venueQueryRdr.GetInt32(0);
                string venueName = venueQueryRdr.GetString(1);
                string venueLocation = venueQueryRdr.GetString(2);
                Venue foundVenue = new Venue(venueName, venueLocation, thisVenueId);
                venues.Add(foundVenue);
            }
            venueQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return venues;
    }
    public static Band Find(int band_id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = band_id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int bandId = 0;
        string bandName = "";

        while(rdr.Read())
        {
          bandId = rdr.GetInt32(0);
          bandName = rdr.GetString(1);
        }

        Band newBand = new Band(bandName, bandId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }

        return newBand;
    }
    public static void DeleteAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM bands;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
  }
}
