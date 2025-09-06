using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qwik.Business;

namespace Qwik.WebApp.Pages
{
    [BindProperties(SupportsGet = true)]
    public class QueueModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public QueueModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public DateTime Date { get; set; } = DateTime.Today;
        public List<Appointment> Appointments { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration.GetConnectionString("ApiURl") ?? "");

                var response = await client.GetAsync($"api/appointments/queue/{Date:yyyy-MM-dd}");

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                    return;
                }

                Appointments = await response.Content.ReadFromJsonAsync<List<Appointment>>();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
}
