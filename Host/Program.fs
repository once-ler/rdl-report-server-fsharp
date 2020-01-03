namespace com.eztier.rdl.report
module Host =

  open System
  open System.ServiceModel
  open System.ServiceModel.Web
  open Service.GreeterWcfService

  [<EntryPoint>]
  let main argv =
    printfn "WCF Host %A" argv

    let webBinding = new WebHttpBinding()
    let webAddress = new Uri("http://localhost:8081")
    let host = new WebServiceHost(typeof<GreeterWcfService>)

    host.AddServiceEndpoint(typeof<IGreeterWcfService>, webBinding, webAddress)
    host.Open()

    printfn("Type [CR] to stop...")
    let mutable press = Console.ReadLine()
    host.Close()

    0
