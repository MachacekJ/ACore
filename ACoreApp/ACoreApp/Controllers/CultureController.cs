using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ACoreApp.Controllers
{
  /// <summary>
  /// This controller is dedicated to set up the current culture in a Browser cookie.
  /// See https://docs.microsoft.com/en-US/aspnet/core/blazor/globalization-localization?view=aspnetcore-9.0#provide-ui-to-choose-the-culture
  /// for more information.
  /// </summary>
  [Route("[controller]/[action]")]
  public class CultureController : Controller
  {
    public IActionResult SetCulture(int lcid, string redirectUri)
    {
      var cul = new CultureInfo(lcid);
      HttpContext.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(
          new RequestCulture(cul.Name)));


      return LocalRedirect(redirectUri);
    }
  }
}