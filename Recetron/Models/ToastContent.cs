using Microsoft.AspNetCore.Components;

using Recetron.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetron.Models
{
  public record ToastContent
  {
    public ToastContent(string title, string content)
    {
      Title = title;
      Content = content;
    }
    public string Title { get; init; }
    public string Content { get; init; }
    public int Duration { get; init; } = 3500;
    public Toast Kind { get; init; } = Toast.Normal;
    public bool IsHtml { get; init; } = false;

    /// <summary>
    /// Kinda Hacky...
    /// </summary>
    public bool IsShowing { get; set; } = true;


    public string KindToString()
    {
      return Kind switch
      {
        Toast.Normal => "",
        Toast.Primary => "toast-primary",
        Toast.Warning => "toast-warning",
        Toast.Error => "toast-error",
        Toast.Success => "toast-success",
        _ => ""
      };
    }
  }
}