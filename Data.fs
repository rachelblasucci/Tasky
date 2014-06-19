namespace Tasky 
open FSharp.Data.Sql
open System
open System.IO

module Data = 

    type sql = SqlDataProvider<ConnectionString = @"Data Source=/Users/rachel/Dropbox/Code/Github/Tasky/Resources/task.sqlite;Version=3;",
                               DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
                               ResolutionPath = @"/Library/Frameworks/Mono.framework/Libraries/mono/4.5/",
                               UseOptionTypes = false>

    type task = { Description : string; mutable Complete : bool; }

    let ctx = sql.GetDataContext()

    let GetIncompleteTasks() = 
        ctx.``[main].[tasks]``
            |> Seq.filter (fun t -> t.complete = 0L)
            |> Seq.map (fun t -> {Description=t.task; Complete=false})
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
