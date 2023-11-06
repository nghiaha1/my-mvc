using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyMvc.Data;
using Newtonsoft.Json;
using ContentDispositionHeaderValue = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue;

namespace MyMvc.Controllers;

[Route("api/[controller]")]
public class BookController : Controller
{
    private HttpClient client = new HttpClient();

    // GET
    public async Task<IActionResult> Index()
    {
        // Thực hiện yêu cầu GET đến URL của API
        var response = await client.GetAsync("https://demo-azure-1815.azurewebsites.net/api/Products");

        // Kiểm tra trạng thái của phản hồi
        var books = new List<Book>();
        var data = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Dữ liệu trả về được lưu trữ trong đối tượng HttpResponseMessage

            // Chuyển đổi dữ liệu JSON sang đối tượng User
            books = JsonConvert.DeserializeObject<List<Book>>(data);
        }

        return View(books);
    }

    [Route("Create")]
    public async Task<IActionResult> Create(Book model)
    {
        var book = new Book
            { Title = model.Title, Description = model.Description, Price = model.Price, Quantity = model.Quantity };

        // Tạo đối tượng FormUrlEncodedContent để truyền dữ liệu đến API
        var disposition = new ContentDispositionHeaderValue("form-data");
        disposition.Name = "book";
        var json = JsonConvert.SerializeObject(book);

        var httpRequestMessage = new HttpRequestMessage
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Clear();
        // Thực hiện yêu cầu POST đến URL của API
        var response = await client.PostAsync("https://demo-azure-1815.azurewebsites.net/api/Products",
            httpRequestMessage.Content);
        // Kiểm tra trạng thái của phản hồi
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [Route("Edit/{id}")]
    public async Task<IActionResult> Edit(int id, Book model)
    {
        var res = await client.GetAsync($"https://demo-azure-1815.azurewebsites.net/api/Products/{id}");

        // Kiểm tra trạng thái của phản hồi
        var bk = new Book();
        var data = await res.Content.ReadAsStringAsync();
        if (res.StatusCode == HttpStatusCode.OK)
        {
            // Dữ liệu trả về được lưu trữ trong đối tượng HttpResponseMessage

            // Chuyển đổi dữ liệu JSON sang đối tượng User
            bk = JsonConvert.DeserializeObject<Book>(data);
        }

        var book = new Book
        {
            Id = id, Title = model.Title, Description = model.Description, Price = model.Price,
            Quantity = model.Quantity
        };

        // Tạo đối tượng FormUrlEncodedContent để truyền dữ liệu đến API
        var disposition = new ContentDispositionHeaderValue("form-data");
        disposition.Name = "book";
        var json = JsonConvert.SerializeObject(book);

        var httpRequestMessage = new HttpRequestMessage
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Thực hiện yêu cầu PUT đến URL của API
        var response = await client.PutAsync($"https://demo-azure-1815.azurewebsites.net/api/Products?id={id}",
            httpRequestMessage.Content);
        // Kiểm tra trạng thái của phản hồi
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(bk);
    }

    [Route("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        // Thực hiện yêu cầu GET đến URL của API
        var response = await client.GetAsync($"https://demo-azure-1815.azurewebsites.net/api/Products/{id}");

        // Kiểm tra trạng thái của phản hồi
        var book = new Book();
        var data = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Dữ liệu trả về được lưu trữ trong đối tượng HttpResponseMessage

            // Chuyển đổi dữ liệu JSON sang đối tượng User
            book = JsonConvert.DeserializeObject<Book>(data);
        }

        return View(book);
    }

    [Route("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await client.DeleteAsync($"https://demo-azure-1815.azurewebsites.net/api/Products/{id}");

        // Kiểm tra trạng thái của phản hồi
        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Dữ liệu trả về được lưu trữ trong đối tượng HttpResponseMessage
            return RedirectToAction(nameof(System.Index));
        }

        return View();
    }
}