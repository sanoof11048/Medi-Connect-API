using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.UserDTOs;
using Medi_Connect.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medi_Connect_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        //private readonly INurseService _nurseService;
        //private readonly 
        //public NurseController(INurseService nurseService)
        //{
        //    _nurseService = nurseService;
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddNurseProfile(CreateNurseDTO nurseProfile)
        //{
        //    return Ok(await _nurseService.AddNurseProfile(nurseProfile));
        //}
    }
}
