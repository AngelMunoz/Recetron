using Microsoft.AspNetCore.Components;

using Recetron.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recetron.Models
{
  public class ToastContent
  {
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public int Duration { get; set; } = 3500;
    public Toast Kind { get; set; } = Toast.Normal;
    public bool IsHtml { get; set; } = false;

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
