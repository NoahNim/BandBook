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
