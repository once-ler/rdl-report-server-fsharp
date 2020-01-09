namespace com.eztier.rdl.report

module Infrastruture =

  open System
  open System.IO

  open fyiReporting.RDL

  type RdlReport() =
    member private this.getSource file =
      let mutable fs: StreamReader = null
      let mutable prog: string = null
      let workingDirectory = Environment.CurrentDirectory
      printfn "%s" workingDirectory
      let read = 
        try
          fs <- new StreamReader(workingDirectory + "/" + file)
          prog <- fs.ReadToEnd()
        finally
          match fs with null -> fs.Close() | _ -> ()

      try
        read
      with
        | :? System.IO.FileNotFoundException as e -> prog <- null; printfn "%s" e.Message

      prog

    member private this.getPassword =
      "12345678"

    member private this.getReport (prog: string) file =
      let mutable r: Report = null

      try
        let rdlp = new RDLParser(prog)
        let mutable folder = Path.GetDirectoryName(file)
        folder <- match folder with | "" -> Environment.CurrentDirectory | _ -> ""
        rdlp.Folder <- folder
        rdlp.DataSourceReferencePassword <- new NeedPassword(fun _ -> this.getPassword)
        r <- rdlp.Parse()

        match r.ErrorMaxSeverity with
          | a when a > 0 ->
            printfn "%s has the following errors: " file
            for emsg in r.ErrorItems do printfn "%A" emsg
            r.ErrorReset()
            if a > 4 then r <- null
          | _ -> ()  

        match r with
          null -> ()
          | _ ->
            r.Folder <- folder
            r.Name <- Path.GetFileNameWithoutExtension(file)
            r.GetDataSourceReferencePassword <- new NeedPassword(fun _ -> this.getPassword)
      with
        | e -> r <- null; printfn "%s" e.Message; ()

      r

    member this.render file =
      let source = this.getSource(file)
      let mutable report = this.getReport source file
      report.UserID <- "admin"
      use sg = new MemoryStreamGen()
      report.RunRender(sg, OutputPresentationType.HTML)

      sg.GetStream()
