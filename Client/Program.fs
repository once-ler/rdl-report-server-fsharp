namespace com.eztier.rdl.report
module Client =

  open System
  open System.ServiceModel
  open System.Net
  open System.IO

  [<EntryPoint>]
  let main argv =
      printfn "WCF Client %A" argv

      let loop = true
      while loop do
        printfn "\nEnter name: "
        let mutable name = Console.ReadLine()
        let req = WebRequest.Create(String.Format(@"http://localhost:8081/greet/{0}", name))
        use resp = req.GetResponse() 
        use stream = resp.GetResponseStream() 
        use reader = new StreamReader(stream) 
        let responseFromServer = reader.ReadToEnd()     
        printfn "%s" responseFromServer

      0
