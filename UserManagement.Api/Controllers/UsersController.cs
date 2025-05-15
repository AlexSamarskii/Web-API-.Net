// ===== UserManagement.Api =====
// File: Controllers/UsersController.cs

using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UserManagement.Application.DTOs;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CreateUserRequest request)
    {
        try
        {
            _userService.CreateUser(request);
            return Ok("User created successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update/info")]
    public IActionResult UpdateInfo(string login, string name, Gender gender, DateTime? birthday, string requester)
    {
        try
        {
            _userService.UpdateInfo(login, name, gender, birthday, requester);
            return Ok("User updated");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update/password")]
    public IActionResult UpdatePassword(string login, string newPassword, string requester)
    {
        try
        {
            _userService.UpdatePassword(login, newPassword, requester);
            return Ok("Password updated");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update/login")]
    public IActionResult UpdateLogin(string oldLogin, string newLogin, string requester)
    {
        try
        {
            _userService.UpdateLogin(oldLogin, newLogin, requester);
            return Ok("Login updated");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("active")]
    public IActionResult GetActiveUsers(string requester)
    {
        try
        {
            var result = _userService.GetActiveUsers(requester);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get")]
    public IActionResult GetUser(string login, string requester)
    {
        try
        {
            var result = _userService.GetUser(login, requester);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("auth")]
    public IActionResult Authenticate(string login, string password)
    {
        try
        {
            var user = _userService.Authenticate(login, password);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet("older")]
    public IActionResult GetUsersOlderThan(int age, string requester)
    {
        try
        {
            var users = _userService.GetUsersOlderThan(age, requester);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public IActionResult Delete(string login, bool soft, string requester)
    {
        try
        {
            _userService.DeleteUser(login, soft, requester);
            return Ok("User deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("restore")]
    public IActionResult Restore(string login, string requester)
    {
        try
        {
            _userService.RestoreUser(login, requester);
            return Ok("User restored");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
