namespace MessageIt

open Owin
open Microsoft.Owin

type Startup() = 
  
  member x.Configuration(app : IAppBuilder) = 
    app.MapSignalR() |> ignore
    ()