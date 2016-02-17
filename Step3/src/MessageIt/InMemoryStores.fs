namespace MessageIt.Infrastructure

open MessageIt.Domain
open MessageIt.Rop

module InMemoryMessageStore = 
  let getAllMessages() : Message seq = 
    let create text = 
      createMessage 
      <!> createMessageText text
    
    let successfulMessage message =
      match message with 
      | Success (msg,_) -> Some msg 
      | _ -> None

    [create "Hi" 
     create "Hello" 
     create "'Sup!"
     create "Greetings"
     create "Howdy"
     create "HELP!"]
    |> List.choose successfulMessage
    |> List.toSeq