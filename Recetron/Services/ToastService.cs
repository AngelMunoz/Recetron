using System;
using Recetron.Enums;
using Recetron.Interfaces;
using Recetron.Models;

namespace Recetron.Services
{
  public class ToastService : IToastService
  {
    public event EventHandler<ToastContent>? OnShowToast;
    public event EventHandler? ClearAllToasts;

    public void ShowCustom(ToastContent content)
    {
      OnShowToast?.Invoke(this, content);
    }

    public void ShowError(string title, string content, int duration = 3500)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Error,
          Duration = duration
        }
      );
    }

    public void ShowNormal(string title, string content, int duration = 3500)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Duration = duration
        }
      );
    }

    public void ShowPrimary(string title, string content, int duration = 3500)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Primary,
          Duration = duration
        }
      );
    }

    public void ShowSuccess(string title, string content, int duration = 3500)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Success,
          Duration = duration
        }
      );
    }

    public void ShowSimple(string title, Toast kind)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Kind = kind
        }
      );
    }

    public void ShowWarning(string title, string content, int duration = 3500)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Warning,
          Duration = duration
        }
      );
    }
  }
}
