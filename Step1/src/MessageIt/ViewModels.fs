namespace MessageIt.ViewModels

open MessageIt.Domain

// ============================== 
// View Models for MVC Pages
// ============================== 

/// ViewModel is a Record Type NOT a Class Type.  Record Types can have members...in this case it is static.  Use with caution.
type MessageViewModel = 
  { Messages : seq<string> }
  static member ToMessageViewModel messages = { Messages = messages |> Seq.map (fun msg -> msg.Text) }
