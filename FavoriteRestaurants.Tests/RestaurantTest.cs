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
      Cuisine.DeleteAll();
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
    public void GetAll_ListofRestaurants_RestaurantList()
    {
      Restaurant newRestaurant = new Restaurant("otherplace", "thatlocation", "23", 2);
      newRestaurant.Save();
      Restaurant newRestaurant2 = new Restaurant("place", "location", "24", 1);
      newRestaurant2.Save();
      List<Restaurant> allRestaurants = Restaurant.GetAll();
      List<Restaurant> expectedList = new List<Restaurant>{newRestaurant, newRestaurant2};
      CollectionAssert.AreEqual(allRestaurants, expectedList);
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

    [TestMethod]
    public void Update_UpdatesRestaurantNameInDatabase_String()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("place", "location", "24", 1);
      testRestaurant.Save();
      string newName = "otherplace";

      //Act
      testRestaurant.UpdateName(newName);

      string result = testRestaurant.GetName();

      //Assert
      Assert.AreEqual(newName, result);
    }

    [TestMethod]
    public void Update_UpdatesRestaurantLocationInDatabase_String()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("place", "location", "24", 1);
      testRestaurant.Save();
      string newLocation = "otherlocation";

      //Act
      testRestaurant.UpdateLocation(newLocation);

      string result = testRestaurant.GetLocation();

      //Assert
      Assert.AreEqual(newLocation, result);
    }

    [TestMethod]
    public void Update_UpdatesRestaurantHoursInDatabase_String()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("place", "location", "24", 1);
      testRestaurant.Save();
      string newHours = "23";

      //Act
      testRestaurant.UpdateHours(newHours);

      string result = testRestaurant.GetHours();

      //Assert
      Assert.AreEqual(newHours, result);
    }

    [TestMethod]
    public void Update_UpdatesRestaurantInDatabase_String()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("place", "location", "24", 1);
      testRestaurant.Save();
      string newName = "otherplace";
      string newLocation = "otherlocation";
      string newHours = "23";

      //Act
      testRestaurant.UpdateRestaurant(newName, newLocation, newHours);

      bool isItTrue = (testRestaurant.GetName() == newName && testRestaurant.GetLocation() == newLocation && testRestaurant.GetHours() == newHours);
      //Assert
      Assert.AreEqual(true, isItTrue);
    }

    [TestMethod]
    public void DeleteRestaurant_RemoveOneRestaurantFromList_RestaurantList()
    {
      Restaurant newRestaurant = new Restaurant("place", "location", "24", 1);
      newRestaurant.Save();
      Restaurant newRestaurant2 = new Restaurant("otherplace", "thatlocation", "23", 2);
      newRestaurant2.Save();

      Restaurant.DeleteRestaurant(newRestaurant.GetId());
      List<Restaurant> allRestaurants = Restaurant.GetAll();
      List<Restaurant> expectedList = new List<Restaurant>{newRestaurant2};

      // Console.WriteLine(allRestaurants[0].GetDetails());
      // Console.WriteLine(expectedList[0].GetDetails());

      CollectionAssert.AreEqual(allRestaurants, expectedList);
    }

    [TestMethod]
    public void GetCuisineName_ReturnsCuisineTypeForRestaurant_String()
    {
      Cuisine newCuisine = new Cuisine("Mexican", 1);
      newCuisine.Save();
      Restaurant newRestaurant = new Restaurant("place", "location", "24", newCuisine.GetId());
      newRestaurant.Save();

      Console.WriteLine(newRestaurant.GetDetails());
      Console.WriteLine(newRestaurant.GetCuisine());

      Assert.AreEqual("Mexican", newRestaurant.GetCuisine());
    }
  }
}
