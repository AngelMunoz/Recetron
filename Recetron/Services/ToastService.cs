using System;
using Recetron.Enums;
using Recetron.Interfaces;
using Recetron.Models;

namespace Recetron.Services
{
  public class ToastService : IToastService
  {
    public event EventHandler<ToastContent>? OnShowToast;

    public void ShowCustom(ToastContent content)
    {
      OnShowToast?.Invoke(this, content);
    }

    public void ShowError(string title, string content)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Error
        }
      );
    }

    public void ShowNormal(string title, string content)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content
        }
      );
    }

    public void ShowPrimary(string title, string content)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Primary
        }
      );
    }

    public void ShowSuccess(string title, string content)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Success
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

    public void ShowWarning(string title, string content)
    {
      OnShowToast?.Invoke(
        this,
        new ToastContent
        {
          Title = title,
          Content = content,
          Kind = Toast.Warning
        }
      );
    }
  }
}
