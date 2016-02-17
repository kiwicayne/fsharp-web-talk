namespace MessageIt.ViewModels

open System
open MessageIt.Domain
open MessageIt.DomainPrimitiveTypes

// ============================== 
// View Models for MVC Pages
// ============================== 
type MessageViewModel = 
  { Messages : seq<string> }
  static member ToMessageViewModel(messages : seq<Message>) = 
    let getMessageText msg = msg.Text |> MessageText.apply id
    { Messages = messages |> Seq.map getMessageText }
