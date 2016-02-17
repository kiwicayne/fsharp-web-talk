namespace MessageIt

open Microsoft.AspNet.SignalR
open ImpromptuInterface.FSharp

module Hubs =

  type EmergencyHub() =
    inherit Hub ()
   
  let notifyEmergencyServices (name,msg) =        
    let hubContext = GlobalHost.ConnectionManager.GetHubContext<EmergencyHub>()
    hubContext.Clients.All?showAlert(name, msg)
        