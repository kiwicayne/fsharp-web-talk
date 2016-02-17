namespace MessageIt.Infrastructure

open MessageIt.Domain

// I opted not to use the word Repository, which is overused in OOP.
module InMemoryMessageStore = 
  let getAllMessages() : Message seq = 
    [ { Text = "Howdy" }
      { Text = "Hello" }
      { Text = "'Sup!" } ]
    |> List.toSeq
