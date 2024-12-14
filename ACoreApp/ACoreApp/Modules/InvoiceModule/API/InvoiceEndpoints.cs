// namespace ACoreApp.Modules.InvoiceModule.API;
//
// public static class InvoiceEndpoints
// {
//   private static string BasePath => "api/localization";
//
//   public static void MapTest(this IEndpointRouteBuilder app)
//   {
//     app.MapGet(BasePath + "/test", async (LocalizationGetQuery req, ISender sender) =>
//     {
//       // HttpContext
//       var aa = await sender.Send(req);
//       return Results.Ok(aa);
//
//     });
//   }
// }