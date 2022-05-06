using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTApi.Controllers
{
    public class BaseeController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(Response<T> response ) where T: class
        {
            return new ObjectResult(response)
            { 
                StatusCode = response.StatusCode
                
            }                            ;
        }
    }
}
