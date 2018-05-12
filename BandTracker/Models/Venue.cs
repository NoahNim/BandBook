using System;
using MySql.Data.MySqlClient;
using BandTracker;
using System.Collections.Generic;

namespace BandTracker.Models
{
  public class Venue
  {
      private int _venue_id;
      private string _name;
      private string _location;

      public Venue(string name, string location, int venue_id = 0)
      {
        _venue_id = venue_id;
        _name = name;
        _location = location;
      }

      public string GetName()
      {
          return _name;
      }
      public int GetId()
      {
          return _venue_id;
      }
      public string GetLocation()
      {
          return _location;
      }
      public override bool Equals(System.Object otherVenue)
      {
        if (!(otherVenue is Venue))
        {
          return false;
        }
        else
        {
           Venue newVenue = (Venue) otherVenue;
           bool idEquality = this.GetId() == newVenue.GetId();
           bool nameEquality = this.GetName() == newVenue.GetName();
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
          cmd.CommandText = @"INSERT INTO venues (name, location) VALUES (@name, @location);";

          MySqlParameter name = new MySqlParameter();
          name.ParameterName = "@name";
          name.Value = this._name;
          cmd.Parameters.Add(name);

          MySqlParameter location = new MySqlParameter();
          location.ParameterName = "@location";
          location.Value = this._location;
          cmd.Parameters.Add(location);

          cmd.ExecuteNonQuery();
          _venue_id = (int) cmd.LastInsertedId;
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
      }
      public static List<Venue> GetAll()
      {
           List<Venue> allVenues = new List<Venue> {};
           MySqlConnection conn = DB.Connection();
           conn.Open();
           var cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT * FROM venues;";
           var rdr = cmd.ExecuteReader() as MySqlDataReader;
           while(rdr.Read())
           {
             int venueId = rdr.GetInt32(0);
             string venueName = rdr.GetString(1);
             string venueLocation = rdr.GetString(2);

             Venue newVenue = new Venue(venueName, venueLocation, venueId);
             allVenues.Add(newVenue);
           }
           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
           return allVenues;
    }
    public void AddBand(Band newBand)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO tracker_info (band_id, venue_id) VALUES (@bandId, @venueId);";

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@bandId";
        band_id.Value = newBand.GetId();
        cmd.Parameters.Add(band_id);

        MySqlParameter venue_id = new MySqlParameter();
        venue_id.ParameterName = "@venueId";
        venue_id.Value = _venue_id;
        cmd.Parameters.Add(venue_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public List<Band> GetBands()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT bands.* FROM venues
            JOIN tracker_info ON (venue_id = tracker_info.venue_id)
            JOIN bands ON (tracker_info.band_id = band_id)
            WHERE venue_id = @venueId;";

        MySqlParameter venueIdParameter = new MySqlParameter();
        venueIdParameter.ParameterName = "@venueId";
        venueIdParameter.Value = _venue_id;
        cmd.Parameters.Add(venueIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> bandIds = new List<int> {};
        while(rdr.Read())
        {
            int bandId = rdr.GetInt32(0);
            bandIds.Add(bandId);
        }
        rdr.Dispose();

        List<Band> bands = new List<Band> {};
        foreach (int bandId in bandIds)
        {
            var bandQuery = conn.CreateCommand() as MySqlCommand;
            bandQuery.CommandText = @"SELECT * FROM bands WHERE id = @BandId;";

            MySqlParameter bandIdParameter = new MySqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = bandId;
            bandQuery.Parameters.Add(bandIdParameter);

            var bandQueryRdr = bandQuery.ExecuteReader() as MySqlDataReader;
            while(bandQueryRdr.Read())
            {
                int thisBandId = bandQueryRdr.GetInt32(0);
                string bandName = bandQueryRdr.GetString(1);
                Band foundBand = new Band(bandName, thisBandId);
                bands.Add(foundBand);
            }
            bandQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return bands;
    }
    public void UpdateVenue(string VenueName, string VenueLocation)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET  name= @newName, location = @newLocation WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _venue_id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = VenueName;
      cmd.Parameters.Add(name);

      MySqlParameter location = new MySqlParameter();
      location.ParameterName = "@newLocation";
      location.Value = VenueLocation;
      cmd.Parameters.Add(location);

      cmd.ExecuteNonQuery();
      _name = VenueName;
      _location = VenueLocation;

      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public static Venue Find(int venue_id)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = venue_id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int venueId = 0;
        string venueName = "";
        string venueLocation = "";

        while(rdr.Read())
        {
          venueId = rdr.GetInt32(0);
          venueName = rdr.GetString(1);
        }

        Venue newVenue = new Venue(venueName, venueLocation, venueId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }

        return newVenue;
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues WHERE id = @venueId; DELETE FROM tracker_info WHERE id = @VenueId;";

      MySqlParameter venueIdParameter = new MySqlParameter();
      venueIdParameter.ParameterName = "@VenueId";
      venueIdParameter.Value = this.GetId();
      cmd.Parameters.Add(venueIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM venues;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
  }
}
