namespace com.eztier.rdl.report

module Service =

  open System
  open System.ServiceModel
  open System.ServiceModel.Web
  open System.ServiceModel.Activation
  open System.Text
  open System.IO

  open Contract
  open Infrastruture

  [<ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)>]   
  [<AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)>]
  type GreeterWcfService() =
    interface IGreeterWcfService with
      member this.Greet name =
        let headers = WebOperationContext.Current.IncomingRequest.Headers

        for headerName in headers.AllKeys do
          printfn "headerName: %s" headerName

        "Hello, " + name

      member this.GetReport name =
        WebOperationContext.Current.OutgoingResponse.ContentType <- "text/html"

        let rrp = new RdlReport()

        let str = rrp.render("Reports/Employees.rdl")
        let bytes = Encoding.UTF8.GetBytes str
        let stream = new MemoryStream(bytes)

        let clientContext = OperationContext.Current
        let handler = new EventHandler(fun obj args ->
          match stream with
            null -> () 
            | _ -> stream.Dispose()
        )
        clientContext.OperationCompleted.AddHandler(handler)

        /// upcast
        stream :> Stream
