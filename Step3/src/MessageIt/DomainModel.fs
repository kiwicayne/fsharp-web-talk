module MessageIt.Domain

open Rop
open DomainPrimitiveTypes

// ============================== 
// Domain models
// ============================== 
type Message = 
  { Text : MessageText.T }

type Person = 
  { Name : PersonName.T }

type MessageRequest = 
  { Person : Person
    Message : Message }

type FormattedMessage = 
  { MessageRequest: MessageRequest
    Content : string }

// All possible things that can happen in the system
type DomainMessage =
  
  // Validation Errors
  | PersonNameIsRequired
  | PersonNameMustNotBeMoreThan20Chars
  | PersonNameMustContainOnlyLettersAndSpaces
  | MessageTextIsRequired
  | MessageTextMustNotBeMoreThan140Chars
  
  // Events 
  | EmergencyMessageSent of string * string
  


// ============================== 
// Domain functions
// ============================== 

let createName name =
  let map = function
    | Missing -> PersonNameIsRequired
    | MustNotBeLongerThan _ -> PersonNameMustNotBeMoreThan20Chars
    | DoesntMatchPattern _ -> PersonNameMustContainOnlyLettersAndSpaces

  PersonName.create name
  |> mapMessages map

let createPerson name =
  { Name = name }

let createMessageText text =
  let map = function
    | Missing -> MessageTextIsRequired
    | MustNotBeLongerThan _ -> MessageTextMustNotBeMoreThan140Chars
    | DoesntMatchPattern _ -> failwith "not expecting DoesntMatchPattern for message text"

  MessageText.create text
  |> mapMessages map

let createMessage text =
  { Text = text }

let createMessageRequest person message =
  { Person = person; Message = message}

let createFormattedMessage messageRequest = 
  let recipientName = messageRequest.Person.Name |> PersonName.apply id
  let messageText = messageRequest.Message.Text |> MessageText.apply id

  succeed 
    { Content = sprintf "%s %s." messageText recipientName 
      MessageRequest = messageRequest }
