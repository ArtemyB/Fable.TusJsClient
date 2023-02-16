// ts2fable 0.8.0
[<AutoOpen>]
module rec Fable.TusJsClient.Core

open System
open Fable.Core
open Fable.Core.JS
open Browser.Types


let [<Import("isSupported", "tus-js-client")>] isSupported: bool = jsNative
let [<Import("canStoreURLs", "tus-js-client")>] canStoreURLs: bool = jsNative
let [<Import("defaultOptions", "tus-js-client")>] defaultOptions: UploadOptions = jsNative

type [<AllowNullLiteral>] Error =
    abstract message : string
    abstract name : string
    abstract stack : string option

type [<AllowNullLiteral>] IExports =
    abstract Upload: UploadStatic
    abstract DefaultHttpStack: DefaultHttpStackStatic
    abstract DetailedError: DetailedErrorStatic

type [<AllowNullLiteral>] Upload =
    abstract file: U3<File, Blob, obj> with get, set
    abstract options: UploadOptions with get, set
    abstract url: string option with get, set
    abstract start: unit -> unit
    abstract abort: ?shouldTerminate: bool -> Promise<unit>
    abstract findPreviousUploads: unit -> Promise<PreviousUpload[]>
    abstract resumeFromPreviousUpload: previousUpload: PreviousUpload -> unit

type [<AllowNullLiteral>] UploadStatic =
    [<EmitConstructor>] abstract Create: file: U3<File, Blob, obj> * options: UploadOptions -> Upload
    abstract terminate: url: string * ?options: UploadOptions -> Promise<unit>

type [<AllowNullLiteral>] UploadOptions =
    abstract endpoint: string option with get, set
    abstract uploadUrl: string option with get, set
    abstract metadata: UploadOptionsMetadata option with get, set
    abstract fingerprint: file: File * ?options: UploadOptions -> Promise<string>
    abstract uploadSize: float option with get, set
    abstract onProgress: (float -> float -> unit) option with get, set
    abstract onChunkComplete: (float -> float -> float -> unit) option with get, set
    abstract onSuccess: (unit -> unit) option with get, set
    abstract onError: (U2<Error, DetailedError> -> unit) option with get, set
    abstract onShouldRetry: (U2<Error, DetailedError> -> float -> UploadOptions -> bool) option with get, set
    abstract onUploadUrlAvailable: (unit -> unit) option with get, set
    abstract overridePatchMethod: bool option with get, set
    abstract headers: UploadOptionsMetadata option with get, set
    abstract addRequestId: bool option with get, set
    abstract onBeforeRequest: req: HttpRequest -> unit
    abstract onAfterResponse: req: HttpRequest * res: HttpResponse -> unit
    abstract chunkSize: float option with get, set
    abstract retryDelays: float[] option with get, set
    abstract parallelUploads: float option with get, set
    abstract parallelUploadBoundaries: {| start: float; ``end``: float |}[] option with get, set
    abstract storeFingerprintForResuming: bool option with get, set
    abstract removeFingerprintOnSuccess: bool option with get, set
    abstract uploadLengthDeferred: bool option with get, set
    abstract uploadDataDuringCreation: bool option with get, set
    abstract urlStorage: UrlStorage option with get, set
    abstract fileReader: FileReader option with get, set
    abstract httpStack: HttpStack option with get, set

type [<AllowNullLiteral>] UrlStorage =
    abstract findAllUploads: unit -> Promise<PreviousUpload[]>
    abstract findUploadsByFingerprint: fingerprint: string -> Promise<PreviousUpload[]>
    abstract removeUpload: urlStorageKey: string -> Promise<unit>
    abstract addUpload: fingerprint: string * upload: PreviousUpload -> Promise<string>

type [<AllowNullLiteral>] PreviousUpload =
    abstract size: float option with get, set
    abstract metadata: UploadOptionsMetadata with get, set
    abstract creationTime: string with get, set

type [<AllowNullLiteral>] FileReader =
    abstract openFile: input: obj option * chunkSize: float -> Promise<FileSource>

type [<AllowNullLiteral>] FileSource =
    abstract size: float with get, set
    abstract slice: start: float * ``end``: float -> Promise<SliceResult>
    abstract close: unit -> unit

type [<AllowNullLiteral>] SliceResult =
    abstract value: obj option with get, set
    abstract ``done``: bool with get, set

type [<AllowNullLiteral>] DefaultHttpStack =
    inherit HttpStack
    abstract createRequest: method: string * url: string -> HttpRequest
    abstract getName: unit -> string

type [<AllowNullLiteral>] DefaultHttpStackStatic =
    [<EmitConstructor>] abstract Create: options: obj option -> DefaultHttpStack

type [<AllowNullLiteral>] HttpStack =
    abstract createRequest: method: string * url: string -> HttpRequest
    abstract getName: unit -> string

type [<AllowNullLiteral>] HttpRequest =
    abstract getMethod: unit -> string
    abstract getURL: unit -> string
    abstract setHeader: header: string * value: string -> unit
    abstract getHeader: header: string -> string
    abstract setProgressHandler: handler: (float -> unit) -> unit
    abstract send: body: obj option -> Promise<HttpResponse>
    abstract abort: unit -> Promise<unit>
    abstract getUnderlyingObject: unit -> obj option

type [<AllowNullLiteral>] HttpResponse =
    abstract getStatus: unit -> float
    abstract getHeader: header: string -> string
    abstract getBody: unit -> string
    abstract getUnderlyingObject: unit -> obj option

type [<AllowNullLiteral>] DetailedError =
    
    abstract originalRequest: HttpRequest with get, set
    abstract originalResponse: HttpResponse with get, set
    abstract causingError: Error with get, set

type [<AllowNullLiteral>] DetailedErrorStatic =
    [<EmitConstructor>] abstract Create: unit -> DetailedError

type [<AllowNullLiteral>] UploadOptionsMetadata =
    [<EmitIndexer>] abstract Item: key: string -> string with get, set