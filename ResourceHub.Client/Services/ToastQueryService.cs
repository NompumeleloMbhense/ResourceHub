using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

/// <summary>
/// A service for handling toast notifications based on query parameters in the URL.
/// This service checks for specific query parameters (e.g., "toast") and displays corresponding
/// toast notifications using the AlertService.
/// </summary>

namespace ResourceHub.Client.Services
{
    public class ToastQueryService
    {
        private readonly AlertService _alert;

        public ToastQueryService(AlertService alert)
        {
            _alert = alert;
        }

        public async Task HandleAsync(NavigationManager nav)
        {

            Console.WriteLine("Toast handler running");

            var uri = nav.ToAbsoluteUri(nav.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (!query.TryGetValue("toast", out var toast))
                return;

            switch (toast.ToString())
            {
                case "resource-created":
                    await _alert.ToastSuccess("Resource created successfully");
                    break;

                case "resource-deleted":
                    await _alert.ToastSuccess("Resource deleted successfully");
                    break;

                case "booking-created":
                    await _alert.ToastSuccess("Booking created successfully");
                    break;

                case "error":
                    await _alert.ToastError("Something went wrong");
                    break;
            }

            // Clean URL after showing toast
            nav.NavigateTo(nav.Uri.Split('?')[0], replace: true);
        }
    }
}