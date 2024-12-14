using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Repositories.EF.PG;
using ACore.Services.Localization.Models;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable PropertyCanBeMadeInitOnly.Global

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace ACore.Server.Modules.LocalizationModule.Repositories.EF.Models;

/// <summary>
/// Entity use only PG repository. You will have to configure names for new repository type separately. Take a look <see cref="DefaultNames"/>
/// </summary>
[Table("localization")]
internal class ACoreLocalizationEntity
{
  [Key]
  [Column("localization_id")]
  public int Id { get; set; }

  [Column("msgid")]
  [MaxLength(255)]
  public string Key { get; set; }

  [Column("translation")]
  public string Translation { get; set; }

  [Column("lcid")]
  public int Lcid { get; set; }

  [Column("contextid")]
  public string ContextId { get; set; }

  [Column("scope")]
  public ACoreLocalizationScopeEnum Scope { get; set; }

  [Column("changed")]
  public DateTime? Changed { get; set; }
}

internal static class LocalizationEntityExtensions
{
  public static ACoreLocalizationItem ToLocalizationRecord(this ACoreLocalizationEntity localizationEntity) => new(Type.GetType(localizationEntity.ContextId) ?? throw new NullReferenceException($"Unknown type for localization contextId: {localizationEntity.ContextId}"), localizationEntity.Key, localizationEntity.Lcid, localizationEntity.Translation);
}