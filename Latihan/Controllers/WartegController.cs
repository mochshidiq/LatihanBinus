using Latihan.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Latihan.Controllers
{
    public class RequestModel
    {
        public Food Food { get; set; }
        public Topping Topping { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class WartegController : ControllerBase
    {
        private readonly WartegContext _context;
        private readonly ILogger<WartegController> _logger;
        public WartegController(ILogger<WartegController> logger,
            WartegContext _context)
        {
            _logger = logger;
            this._context = _context;
        }
        [Authorize]
        [HttpPost]
        [Route("StoreNewMenu")]
        public IActionResult Post(RequestModel Model)
        {
            try
            {
                using (var db = _context)
                {
                    Model.Food.FoodId = 0;
                    // Create a new food and toping and save it
                    db.Toppings.AddAsync(Model.Topping);
                    db.SaveChanges();
                    Model.Food.ToppingId = Model.Topping.ToppingId;
                    db.Foods.AddAsync(Model.Food);
                    var Save = db.SaveChangesAsync();
                    Task.WhenAll(Save);
                    return Ok(Model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("UpdateExistingMenu")]
        public IActionResult Update(Food Food)
        {
            try
            {
                using (var db = _context)
                {
                    if (!db.Foods.Any(x => x.FoodId == Food.FoodId))
                    {
                        return BadRequest("No data exist");
                    }
                    db.Foods.Update(Food);
                    db.SaveChanges();
                    return Ok(Food);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetAllMenu")]
        public IActionResult Get()
        {
            try
            {
                using (var db = _context)
                {
                    var FoodList = (from b in db.Foods
                                    orderby b.Name
                                    select b).ToList();
                    return Ok(FoodList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetPagingMenu")]
        public IActionResult GetPaging([FromQuery] int start,
            [FromQuery] int take)
        {
            try
            {
                using (var db = _context)
                {
                    var FoodList = (from b in db.Foods
                                    orderby b.Name
                                    select b)
                                    .Skip(start)
                                    .Take(take)
                                    .ToList();
                    return Ok(FoodList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("HardDeleteFood")]
        public IActionResult DeleteFood([FromQuery] int FoodId)
        {
            try
            {
                using (var db = _context)
                {
                    var SelectedFood = db.Foods.Where(x => x.FoodId == FoodId).FirstOrDefault();
                    db.Remove<Food>(SelectedFood);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("HardDeleteTopping")]
        public IActionResult DeleteTopping([FromQuery] int ToppingId)
        {
            try
            {
                using (var db = _context)
                {
                    var SelectedTopping = db.Toppings.Where(x => x.ToppingId == ToppingId).FirstOrDefault();
                    db.Remove<Topping>(SelectedTopping);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
