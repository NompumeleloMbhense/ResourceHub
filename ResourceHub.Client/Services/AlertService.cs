using Microsoft.JSInterop;

/// <summary>
/// A service for displaying alerts and confirmation dialogs using JavaScript interop.
/// This service provides methods for showing success, error and info toasts, as
/// confirmation dialogs, and other alert types as needed.
/// </summary>

namespace ResourceHub.Client.Services
{
    public class AlertService
    {
        private readonly IJSRuntime _js;

        public AlertService(IJSRuntime js)
        {
            _js = js;
        }

        public ValueTask ToastSuccess(string message)
            => _js.InvokeVoidAsync("alerts.toast", "success", message);

        public ValueTask ToastError(string message)
            => _js.InvokeVoidAsync("alerts.toast", "error", message);

        public ValueTask ToastInfo(string message)
            => _js.InvokeVoidAsync("alerts.toast", "info", message);

        public async Task<bool> Confirm(string title, string text)
        {
            var result = await _js.InvokeAsync<SwalResult>("alerts.confirm", title, text);
            return result.IsConfirmed;
        }

        private class SwalResult
        {
            public bool IsConfirmed { get; set; }
        }
    }
}