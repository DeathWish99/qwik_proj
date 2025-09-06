using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Qwik.WebApp.Pages
{
    [BindProperties]
    public class SettingsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SettingsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public int MaxAppointments { get; set; }
        public DateTime OffDay { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task OnPostUpdateMaxAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration.GetConnectionString("ApiURl") ?? "");

                var response = await client.PutAsync($"api/agencysettings/max?max={MaxAppointments}", null);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                    return;
                }

                SuccessMessage = "Max updated!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        public async Task OnPostAddOffDayAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://your-api-url/");

                var response = await client.PostAsync($"api/agencysettings/offday?offDay={OffDay:yyyy-MM-dd}", null);

                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                    return;
                }

                SuccessMessage = "Off day added!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }
}
