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
