namespace MessageIt.Dtos

// ============================== 
// DTOs for API
// ============================== 

// CLIMutable - Allows an instance of the type to be populated, 
// but still cannot assign to the properties on this type 
// e.g. myDto.Name <- "Jim" is a compile error.  So still have 
// immutable benefits

/// A message to be sent (or could also represent one that has been sent)
[<CLIMutable>] 
type MessageDto = 
  { Name : string
    Message : string }
