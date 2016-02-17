namespace MessageIt.Dtos

open MessageIt.Rop
open MessageIt.Domain

// ============================== 
// DTOs for API
// ============================== 

/// A message to be sent (or could also represent one that has been sent)
[<CLIMutable>]
type MessageRequestDto = 
  { Name : string
    Message : string }

// ============================== 
// DTO Converters
// ============================== 

module DtoConverter = 
  
  /// Convert a DTO into a domain MessageRequest
  ///
  /// Due to strong typing on domain model, validation is performed just
  /// by creating the domain model.  No need for a separate step.
  ///
  /// <!> is lift (aka map)
  /// <*> is apply
  ///
  /// Pattern is always <!> for first param, then <*> for each additional param
  let dtoToMessageRequest (dto: MessageRequestDto) =
    let personOrError = 
      createPerson
      <!> createName dto.Name
    
    let messageOrError = 
      createMessage 
      <!> createMessageText dto.Message      

    createMessageRequest 
      <!> personOrError
      <*> messageOrError    
    
