using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using FavoriteRestaurants.Models;

namespace FavoriteRestaurants.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      List<Cuisine> allCuisines = Cuisine.GetAll();
      return View(allCuisines);
    }

    [HttpGet("/cuisines/form")]
    public ActionResult FormCuisine()
    {
      return View();
    }

    [HttpPost("/cuisines")]
    public ActionResult CuisineList()
    {
      string cuisineInput = Request.Form["cuisine-name"];
      string cuisineUpper = char.ToUpper(cuisineInput[0]) + cuisineInput.Substring(1);
      Cuisine newCuisine = new Cuisine(cuisineUpper);
      newCuisine.Save();
      List<Cuisine> allCuisines = Cuisine.GetAll();
      return View("Index", allCuisines);
    }

    [HttpGet("/cuisines/delete-all")]
    public ActionResult CuisineDeleteAll()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
      List<Cuisine> allCuisines = Cuisine.GetAll();
      return View("Index", allCuisines);
    }

    [HttpGet("/cuisines/{id}/{name}-restaurants/edit")]
    public ActionResult CuisineUpdate(int id)
    {
      Cuisine thisCuisine = Cuisine.Find(id);
      return View(thisCuisine);
    }

    [HttpPost("/cuisines/{id}/{name}-restaurants/update")]
    public ActionResult CuisineEditList(int id)
    {
      Cuisine thisCuisine = Cuisine.Find(id);
      thisCuisine.UpdateCuisineName(Request.Form["cuisine-name"]);
      return RedirectToAction("Restaurants");
    }


    [HttpGet("/cuisines/{id}/{name}-restaurants")]
    public ActionResult Restaurants(int id)
    {
      Cuisine thisCuisine = Cuisine.Find(id);
      List<Restaurant> allRestaurants = thisCuisine.GetRestaurants();
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("cuisines", thisCuisine);
      model.Add("restaurants", allRestaurants);
      return View(model);
    }

    [HttpGet("/cuisines/{id}/{name}-restaurants/form")]
    public ActionResult RestaurantForm(int id)
    {
      Cuisine thisCuisine = Cuisine.Find(id);
      return View("RestaurantForm", thisCuisine);
    }

    [HttpPost("/cuisines/{id}/{name}-restaurants/new")]
    public ActionResult RestaurantList(int id)
    {
      Cuisine thisCuisine = Cuisine.Find(id);
      Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], Request.Form["location"], Request.Form["hours"], id);
      newRestaurant.Save();
      List<Restaurant> allRestaurants = thisCuisine.GetRestaurants();
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("cuisines", thisCuisine);
      model.Add("restaurants", allRestaurants);
      return View("Restaurants", model);
    }

    [HttpGet("/cuisines/{id}/{name}-restaurants/{id2}")]
    public ActionResult RestaurantDetails(int id2)
    {
      Restaurant thisRestaurant = Restaurant.Find(id2);
      return View(thisRestaurant);
    }

    [HttpGet("/cuisines/{id}/{name}-restaurants/{id2}/edit")]
    public ActionResult RestaurantEdit(int id2)
    {
      Restaurant thisRestaurant = Restaurant.Find(id2);
      return View(thisRestaurant);
    }

    [HttpPost("/cuisines/{id}/{name}-restaurants/{id2}/update")]
    public ActionResult RestaurantUpdatedList(int id2)
    {
      Restaurant thisRestaurant = Restaurant.Find(id2);
      thisRestaurant.UpdateRestaurant(Request.Form["restaurant-name"], Request.Form["location"], Request.Form["hours"]);
      return RedirectToAction("Restaurants");
    }

    [HttpGet("/cuisines/{id}/{name}-restaurants/{id2}/delete")]
    public ActionResult RestaurantDelete(int id, int id2)
    {
      Restaurant.DeleteRestaurant(id2);
      Cuisine thisCuisine = Cuisine.Find(id);
      List<Restaurant> allRestaurants = thisCuisine.GetRestaurants();
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("cuisines", thisCuisine);
      model.Add("restaurants", allRestaurants);
      return View("Restaurants", model);
    }
  }
}
