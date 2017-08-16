using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace FavoriteRestaurants.Models
{
  public class Cuisine
  {
    private int _id;
    private string _name;

    public Cuisine(string name, int id=0)
    {
      _id = id;
      _name = name;
    }

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetDetails()
   {
     return "ID: " +_id + ", Name: " + _name;
   }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> allCuisine = new List<Cuisine> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines ORDER BY name ASC;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int CuisineId = rdr.GetInt32(0);
        string CuisineName = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(CuisineName, CuisineId);
        allCuisine.Add(newCuisine);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCuisine;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cuisines;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if(!(otherCuisine is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;
        bool idEquality = (this.GetId() == newCuisine.GetId());
        bool nameEquality = (this.GetName() == newCuisine.GetName());
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
      cmd.CommandText = @"INSERT INTO cuisines (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Cuisine Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CuisineId = 0;
      string CuisineName = "";

      while(rdr.Read())
      {
        CuisineId = rdr.GetInt32(0);
        CuisineName = rdr.GetString(1);
      }
      Cuisine newCuisine = new Cuisine(CuisineName, CuisineId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCuisine;
    }

    public List<Restaurant> GetRestaurants()
    {
       List<Restaurant> allRestaurants = new List<Restaurant> ();
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"SELECT * FROM restaurants WHERE cuisine_id = @cuisine_id ORDER BY name ASC";

       MySqlParameter cuisineId = new MySqlParameter();
       cuisineId.ParameterName = "@cuisine_id";
       cuisineId.Value = this._id;
       cmd.Parameters.Add(cuisineId);


       var rdr = cmd.ExecuteReader() as MySqlDataReader;
       while(rdr.Read())
       {
         int restaurantId = rdr.GetInt32(0);
         string name = rdr.GetString(1);
         string location = rdr.GetString(2);
         string hours = rdr.GetString(3);
         int fk_cuisineId = rdr.GetInt32(4);
         Restaurant newRestaurant = new Restaurant(name, location, hours, fk_cuisineId , restaurantId);
         allRestaurants.Add(newRestaurant);
       }
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
       return allRestaurants;
    }
  }
}
