using Telerik.SvgIcons;

namespace ACore.Blazor.Abstractions;

public interface IPageConfig : IComponentConfig
{
    string PageId { get; }
    string PageUrl { get; }
    string? TitleLocalizationKey { get; } 
    ISvgIcon? Icon { get; }
}

