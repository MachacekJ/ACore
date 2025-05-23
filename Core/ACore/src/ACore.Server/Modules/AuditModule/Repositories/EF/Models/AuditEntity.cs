﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Repository.Contexts.EF.Models.PK;
using ACore.Server.Repository.Models.EntityEvent;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Repositories.EF.Models;

internal class AuditEntity : PKLongEntity
{
  public int AuditTableId { get; set; }
  public long? PKValue { get; set; }

  [MaxLength(450)]
  public string? PKValueString { get; set; }

  public int? AuditUserId { get; set; }
  
  public DateTime DateTime { get; set; }
  
  public EntityEventEnum EntityState { get; set; }

  [ForeignKey("AuditTableId")]
  public AuditTableEntity AuditTable { get; set; }

  [ForeignKey("AuditUserId")]
  public AuditUserEntity User { get; set; }

  public ICollection<AuditValueEntity> AuditValues { get; set; }
}