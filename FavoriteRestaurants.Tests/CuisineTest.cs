using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using FavoriteRestaurants.Models;

namespace FavoriteRestaurants.Tests
{
  [TestClass]
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=favorite_restaurants_test;";
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
    }

    [TestMethod]
    public void GetAll_CuisinesEmptyAtFirst_0()
    {
     //Arrange, Act
      int result = Cuisine.GetAll().Count;
     //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Cuisine()
    {
      //Arrange, Act
      Cuisine firstCuisine = new Cuisine("Mexican");
      Cuisine secondCuisine = new Cuisine("Mexican");

      //Assert
      Assert.AreEqual(firstCuisine, secondCuisine);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToCuisine_Id()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();

      //Act
      Cuisine savedCuisine = Cuisine.GetAll()[0];

      int result = savedCuisine.GetId();
      int testId = testCuisine.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Save_SavesCuisineToDatabase_CuisineList()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();
      //Act
      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};
      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_ListAllCuisines_CuisineList()
    {
      Cuisine newCuisine = new Cuisine("Japanese");
      newCuisine.Save();
      Cuisine newCuisine2 = new Cuisine("Mexican");
      newCuisine2.Save();
      List<Cuisine> allCuisine = Cuisine.GetAll();
      List<Cuisine> expectedList = new List<Cuisine>{newCuisine, newCuisine2};
      CollectionAssert.AreEqual(allCuisine, expectedList);
    }

    [TestMethod]
    public void Find_FindsCuisineInDatabase_Cuisine()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();

      //Act
      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      //Assert
      Assert.AreEqual(testCuisine, foundCuisine);
    }

    [TestMethod]
    public void GetRestaurants_RetrievesAllRestaurantServingCuisine_RestaurantList()
    {
      Cuisine testCuisine = new Cuisine("Mexican");
      testCuisine.Save();

      Restaurant firstRestaurant = new Restaurant("dinner", "a diner", "nighttime", testCuisine.GetId());
      firstRestaurant.Save();
      Restaurant secondRestaurant = new Restaurant("restaurant", "5th street", "all of em", testCuisine.GetId());
      secondRestaurant.Save();


      List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

      CollectionAssert.AreEqual(testRestaurantList, resultRestaurantList);
    }

    [TestMethod]
    public void UpdateCuisineName_UpdatesCuisineName_CuisineName()
    {
      Cuisine testCuisine = new Cuisine("Mexican", 1);
      testCuisine.Save();
      string newCuisine = "Japanese";
      testCuisine.UpdateCuisineName(newCuisine);
      string result = testCuisine.GetName();
      Assert.AreEqual(newCuisine, result);
    }
  }
}
