namespace MessageIt.Infrastructure

open MessageIt.Domain
open MessageIt.Rop

// I opted not to use the word Repository, which is overused in OOP.
module InMemoryMessageStore = 
  let getAllMessages() : Message seq = 
    let create text = 
      createMessage 
      <!> createMessageText text
    
    let successfulMessage message =
      match message with 
      | Success (msg,_) -> Some msg 
      | _ -> None

    // create returns a RopResult, so need to choose only the successfully
    // created messages
    [create "Hi" 
     create "Hello" 
     create "'Sup!"
     create "Greetings"
     create "Howdy"]
    |> List.choose successfulMessage
    |> List.toSeq