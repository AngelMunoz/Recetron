using Recetron.Enums;
using Recetron.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetron.Interfaces
{

  public interface IToastService
  {
    event EventHandler<ToastContent>? OnShowToast;
    void ShowCustom(ToastContent content);
    void ShowSimple(string title, Toast kind);
    void ShowNormal(string title, string content);
    void ShowPrimary(string title, string content);
    void ShowSuccess(string title, string content);
    void ShowWarning(string title, string content);
    void ShowError(string title, string content);
  }
}
