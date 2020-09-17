using System;
using System.Threading.Tasks;
using Recetron.Core.Models;

namespace Recetron.Interfaces
{
    public interface IInteropService
    {
        Task<bool> HasShareAPI();
        Task ShareMobile(string title, string text, string? url);
    }
}