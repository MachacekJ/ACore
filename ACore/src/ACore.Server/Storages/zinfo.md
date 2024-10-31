# Storages

## Entity framework
Supported types of storages
- InMemory
- Postgres
- Mongo

## Folders
- **Attributes** - attributes for entity class and props decoration. Class for attributes working
```csharp
[CheckSum]
internal class TestNoAuditEntity : PKIntEntity
{
  ...
}
```
- **Configuration** - Function for correct storage registration to IoC in modules.
- **Contexts** - Storage context (Abstract Classes) implementations for supported types
- **CQRS**
  - **Handlers** - 



