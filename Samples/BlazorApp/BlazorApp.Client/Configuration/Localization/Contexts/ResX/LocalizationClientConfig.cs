// using System.Resources;
// using ACore.Blazor.Abstractions;
//
// namespace BlazorApp.Client.Configuration.Localization.Contexts.ResX;
//
// public static class LocalizationResXConfig
// {
//   public static Dictionary<Type, ResourceManager> ResXClients
//   {
//     get
//     {
//       var result = new Dictionary<Type, ResourceManager>();
//       var baseType = typeof(IComponentConfig);
//       var allComponents = AppDomain.CurrentDomain.GetAssemblies()
//         .SelectMany(s => s.GetTypes())
//         .Where(p => baseType.IsAssignableFrom(p) && p is { IsInterface: false, IsAbstract: false, IsClass: true });
//
//       foreach (var type in allComponents)
//       {
//         if (Activator.CreateInstance(type) is IComponentConfig { ResX: not null } componentConfig)
//           result.Add(componentConfig.ResX, new ResourceManager(componentConfig.ResX));
//       }
//
//       return result;
//     }
//   }
// }