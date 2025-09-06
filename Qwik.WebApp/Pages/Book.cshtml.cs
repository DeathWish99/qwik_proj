using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qwik.Business;

namespace Qwik.WebApp.Pages
{
    [BindProperties]
    public class BookModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BookModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public string CustomerName { get; set; }
        public DateTime RequestedDate { get; set; } = DateTime.Today;
        public Appointment Appointment { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration.GetConnectionString("ApiURl") ?? "");

                var request = new { CustomerName, RequestedDate };
                var response = await client.PostAsJsonAsync("api/appointments/book", request);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                    return;
                }

                Appointment = await response.Content.ReadFromJsonAsync<Appointment>();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
}
