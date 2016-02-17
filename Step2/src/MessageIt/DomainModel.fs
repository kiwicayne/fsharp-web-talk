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
  { Content : string }

// All possible things that can happen in the system
// At a glance you can determine every type of error, event, etc that the system
// deals with.  With exceptions in C# you have no idea.  This could be decomposed
// into multiple subtypes for a very large system
type DomainMessage =
  
  // Validation Errors
  | PersonNameIsRequired
  | PersonNameMustNotBeMoreThan20Chars
  | PersonNameMustContainOnlyLettersAndSpaces
  | MessageTextIsRequired
  | MessageTextMustNotBeMoreThan140Chars
  
  // Events - e.g. something happened such as "a message was sent"
  // Internal errors - e.g. database errors such as database timeout


// ============================== 
// Domain functions
// ============================== 
// With a large domain, you would wrap related functions into modules and move them
// into their own files.  Despite being in the same file, the data and functions that 
// operate on that data are separate, unlike OOP.  

/// Create a name, returning a DomainMessage if it fails
let createName name =
  // map a StringError to a DomainMessage, forced to handle all options
  let map = function
    | Missing -> PersonNameIsRequired
    | MustNotBeLongerThan _ -> PersonNameMustNotBeMoreThan20Chars
    | DoesntMatchPattern _ -> PersonNameMustContainOnlyLettersAndSpaces

  // Create the String20 and convert any error message into a DomainMessage
  PersonName.create name
  |> mapMessages map

let createPerson name =
  { Name = name }

let createMessageText text =
  // map a StringError to a DomainMessage, forced to handle all options
  let map = function
    | Missing -> MessageTextIsRequired
    | MustNotBeLongerThan _ -> MessageTextMustNotBeMoreThan140Chars
    | DoesntMatchPattern _ -> failwith "not expecting DoesntMatchPattern for message text"

  // Create the String140 and convert any error message into a DomainMessage
  MessageText.create text
  |> mapMessages map

let createMessage text =
  { Text = text }

let createMessageRequest person message =
  { Person = person; Message = message}

/// create a formatted message from a raw message and a person
let createFormattedMessage messageRequest = 
  // extract the raw string from the wrappers, this would be pretty unusal to need to do
  // but I wanted to show how to do it
  let recipientName = messageRequest.Person.Name |> PersonName.apply id
  let messageText = messageRequest.Message.Text |> MessageText.apply id

  // always succeed, nothing can go wrong
  succeed { Content = sprintf "%s %s." messageText recipientName }
