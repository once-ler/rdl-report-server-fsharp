﻿namespace com.eztier.rdl.report

open System.IO;
open System.ServiceModel;
open System.ServiceModel.Web;

module Contract =

  [<ServiceContract>]
  type IGreeterWcfService =
    [<OperationContract>]
    [<WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "greet/{name}")>]
    abstract member Greet: name: string -> string

    [<OperationContract>]
    [<WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "report/{name}")>]
    abstract member GetReport: name: string -> Stream  
