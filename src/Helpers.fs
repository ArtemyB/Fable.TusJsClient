module Fable.TusJsClient.Helpers

open Fable.Core
open Fable.Core.JsInterop

[<ImportAll("tus-js-client")>]
let tus: IExports = jsNative

let inline ofDefaultOptions (setOptions: UploadOptions -> unit) =
    let o: UploadOptions = !! JS.Constructors.Object.assign(createEmpty, defaultOptions)
    setOptions o
    o


let tusOptions tusEndpoint fileMetadata (onProgress: float -> unit) onSuccess onError =
    ofDefaultOptions (fun opts ->
        opts.endpoint <- Some tusEndpoint
        opts.retryDelays <- Some [| 0.; 1000.; 3000.; 5000. |]
        opts.metadata <- Some fileMetadata
        opts.onError <- Some onError
        opts.onProgress <- Some (fun sent total ->
            onProgress ((sent / total) * 100.) )
        opts.onSuccess <- Some onSuccess
    )


let jsFileMetadata (file: Browser.Types.File) =
    {| 
        fileName = file.name
        fileType = file.``type``
        fileSize = file.size
        lastModified = file.lastModified
    |}


let createFileUpload tusEndpoint onProgress onSuccess onError file =
    let metadata: UploadOptionsMetadata = !! jsFileMetadata file
    let options = tusOptions tusEndpoint metadata onProgress onSuccess onError
    tus.Upload.Create(!^ file, options)


let createBlobUpload tusEndpoint metadata onProgress onSuccess onError blob =
    let options = tusOptions tusEndpoint metadata onProgress onSuccess onError
    tus.Upload.Create(U3.Case2 blob, options)