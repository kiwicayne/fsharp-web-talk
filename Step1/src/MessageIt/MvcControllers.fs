namespace MessageIt.Controllers

open System.Web.Mvc
open MessageIt.Domain
open MessageIt.ViewModels

// ============================== 
// Home
// ============================== 
type HomeController() =
    inherit Controller()
    member this.Index () = this.View()

// ============================== 
// Messenger
// ============================== 
type MessengerController(getMessages : _ -> seq<Message>) = 
  inherit Controller()

  member this.Index() = 
    let vm = getMessages() |> MessageViewModel.ToMessageViewModel
    this.View(vm)
