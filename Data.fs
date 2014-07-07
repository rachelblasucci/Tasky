namespace Tasky 
open FSharp.Data.Sql
open System
open System.IO

module Data = 

    [<Literal>]
    let connectionString = @"Data Source=" + __SOURCE_DIRECTORY__ + @"/Resources/task.sqlite;Version=3;" 

    type sql = SqlDataProvider<ConnectionString = connectionString,
                               DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
                               ResolutionPath = @"/Library/Frameworks/Mono.framework/Libraries/mono/4.5/",
                               UseOptionTypes = false>

    type task = { Description : string; mutable Complete : bool; }

    let private ctx = sql.GetDataContext()

    let GetIncompleteTasks () = 
        query { for data in ctx.``[main].[tasks]`` do 
                    where (data.complete = 0L)
                    select {Description=data.task; Complete = false}}
                |> Seq.toList

    let private findTask description = ctx.``[main].[tasks]``
                                        |> Seq.find (fun t -> t.task = description)

    let AddTask description = 
        let newTask = ctx.``[main].[tasks]``.Create()
        newTask.task <- description
        newTask.complete <- 0L
        ctx.SubmitUpdates()

    let DeleteTask description = 
        let task = findTask description
        task.Delete()
        ctx.SubmitUpdates()

    let UpdateTask description complete = 
        let task = findTask description
        task.complete <- match complete with 
                            | false -> 0L
                            | true -> 1L
        task.task <- description
        ctx.SubmitUpdates()
