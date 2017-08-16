using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FavoriteRestaurants.Models;
using System;

namespace FavoriteRestaurants.Tests
{
  [TestClass]
  public class RestaurantTest : IDisposable
  {
    public void Dispose()
    {
      Restaurant.DeleteAll();
    }

    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=favorite_restaurants_test;";
    }

    [TestMethod]
    public void GetAll_RestaurantsEmptyAtFirst_0()
    {
     //Arrange, Act
      int result = Restaurant.GetAll().Count;
     //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("place", "location", "24", 1);

      //Act
      testRestaurant.Save();
      Restaurant savedRestaurant = Restaurant.GetAll()[0];

      int result = savedRestaurant.GetId();
      int testId = testRestaurant.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Save_SavesToDatabase_RestaurantList()
    {
      //Arrange
      Restaurant newRestaurant = new Restaurant("place", "location", "24", 1);

      //Act
      newRestaurant.Save();
      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{newRestaurant};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_Lists2Restaurants_2()
    {
      Restaurant newRestaurant = new Restaurant("place", "location", "24", 1);
      Restaurant newRestaurant2 = new Restaurant("otherplace", "thatlocation", "23", 2);
      newRestaurant.Save();
      newRestaurant2.Save();
      int result = Restaurant.GetAll().Count;
      Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void Equals_RestaurantsAreEqual_true()
    {
      Restaurant newRestaurant = new Restaurant("place", "location", "24", 1);
      Restaurant newRestaurant2 = new Restaurant("place", "location", "24", 1);
      Assert.AreEqual(newRestaurant, newRestaurant2);
    }

    [TestMethod]
    public void Find_FindsRestaurantInDatabase_Restaurant()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("place", "location", "24", 1);
      testRestaurant.Save();

      //Act
      Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());

      //Assert
      Assert.AreEqual(testRestaurant, foundRestaurant);
    }

  }
}
