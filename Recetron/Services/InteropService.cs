using System;
using System.Threading.Tasks;
using Recetron.Interfaces;
using Microsoft.JSInterop;
namespace Recetron.Services
{
    public class InteropService : IInteropService
    {
        private readonly IJSRuntime jSRuntime;

        public InteropService(IJSRuntime jsruntime)
        {
            jSRuntime = jsruntime;
        }
        public async Task<bool> HasShareAPI()
        {
            return await jSRuntime.InvokeAsync<bool>("Recetron.Interop.hasShareAPI");
        }

        public Task ShareMobile(string title, string text, string? url)
        {
            return jSRuntime.InvokeVoidAsync("Recetron.Interop.shareMobile", new string[] { title, text, url }).AsTask();
        }
    }
}