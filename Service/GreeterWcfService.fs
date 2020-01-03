namespace com.eztier.rdl.report

module Service =

  open System
  open System.IO
  open System.Text
  open System.Globalization
  open System.ServiceModel
  open System.ServiceModel.Web
  open System.ServiceModel.Activation

  open Contracts

  [<AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)>]
  type GreeterWcfService =
    interface IGreeterWcfService with
      member this.Greet name =
        let headers = WebOperationContext.Current.IncomingRequest.Headers

        for headerName in headers.AllKeys do
          printfn "headerName: %s" headerName

        "Hello, " + name

      member this.GetReport reportName =
        let rrp = new RdlReport()

        use stream = rrp.render("Reports/Employees.rdl")

        let clientContext = OperationContext.Current
        let handler = new EventHandler(fun obj args ->
          if stream != null
          then stream.Dispose()
          else ()
        )
        clientContext.OperationCompleted.AddHandler(handler)

        stream