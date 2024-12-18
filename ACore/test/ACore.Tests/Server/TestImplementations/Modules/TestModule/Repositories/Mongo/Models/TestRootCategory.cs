﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Attributes;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models
{
  [Auditable(1)]
  [Table("test_rootcategory")]
  public class TestRootCategory
  {
    [Key]
    [Column("_id")]
    public ObjectId Id { get; set; }
  
    [Column("name")]
    public string Name { get; set; } = null!;
  
    [Column("last_modify")]
    //[ConcurrencyCheck]
    public DateTime LastModify { get; set; }
  
    public ICollection<TestCategory> SubCategories { get; set; }
  }
}

