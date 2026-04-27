using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Proyecto.Data;
using Proyecto.Enums;
using Proyecto.Models;
using Proyecto.Services;
using Proyecto.Services.Interfaces;

namespace Proyecto.Controllers;

public class UserController : Controller
{
    private readonly IUserServices _userService;
    private readonly IBookServices _bookService;
    private readonly ILoanServices _loanService;
    public UserController(IUserServices userService, IBookServices bookService, ILoanServices loanService)
    {
        _userService = userService;
        _bookService = bookService;
        _loanService = loanService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateBook(Books book)
    {
        // aseguramos que la consulta se acople a la del sistema del usuario
        // antes de que llegue a la DB no llegue la fecha NULL
        book.CreatedAt = DateTime.Now;
        book.UpdatedAt = DateTime.Now;
        await _bookService.CreateBook(book);
        return RedirectToAction("Books");
    }
    
    public async Task<IActionResult> Books()
    {
        var response = await _bookService.GetAllBooks();
        return View(response);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.Delete(id);
        return RedirectToAction("Books");
    }
    
    [HttpPost]
    public async Task<IActionResult> EditBook(Books book)
    {
        book.UpdatedAt = DateTime.Now;
        var response = await _bookService.Update(book.Id,book);
        return RedirectToAction("Books");
    }

    // ----------- USUARIOS -----------
    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var response = await _userService.GetAllUsers();
        return View(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create(Users user)
    {
        // aseguramos que la consulta se acople a la del sistema del usuario
        // antes de que llegue a la DB no llegue la fecha NULL
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        await _userService.Create(user);
        return RedirectToAction("Users");
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return RedirectToAction("Users");
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Users user)
    {
        user.UpdatedAt = DateTime.Now;
        await _userService.Update(user.Id,user);
        return RedirectToAction("Users");
    }
    // ------------- PRESTAMOS -------------
    [HttpGet]
    public async Task<IActionResult> Loans()
    {
        
        var responseUsers = await _userService.GetAllUsers();
        var responseBooks = await _bookService.GetAllBooks();
        ViewBag.Users = responseUsers.Data;
        ViewBag.Books = responseBooks.Data.Where(b => b.Status == BookStatus.Available).ToList();;
        var responseloans = await _loanService.GetAllLoans();
        return View(responseloans);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan(Loans loan)
    {
        
        loan.CreatedAt = DateTime.Now;
        loan.UpdatedAt = DateTime.Now;
        await _loanService.CreateLoan(loan);
        return RedirectToAction("Loans");
    }
    [HttpPost]
    public async Task<IActionResult> ReturnLoan(int id)
    {
        await _loanService.ReturnLoan(id);
        return RedirectToAction("Loans");
    }
}