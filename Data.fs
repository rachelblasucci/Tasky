namespace Tasky 
open FSharp.Data.Sql
open System
open System.IO

module Data = 

    [<Literal>]
    let private connectionString = @"Data Source=" + __SOURCE_DIRECTORY__ + @"/Resources/task.sqlite;Version=3;" 

    type sql = SqlDataProvider<ConnectionString = connectionString,
                               DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
                               ResolutionPath = @"/Library/Frameworks/Mono.framework/Libraries/mono/4.5/",
                               UseOptionTypes = false>

    type task = {Id : Int64; Description : string; mutable Complete : bool }

    let private ctx = sql.GetDataContext()

    let GetIncompleteTasks () = 
        query { for data in ctx.``[main].[tasks]`` do 
                    where (data.complete = 0L)
                    select {Id = data.t1key; Description = data.task; Complete = false}}
                |> Seq.toList

    let private findTask id =
        ctx.``[main].[tasks]``
        |> Seq.find (fun t -> t.t1key = id)

    let AddTask description = 
        let newTask = ctx.``[main].[tasks]``.Create()
        newTask.task <- description
        newTask.complete <- 0L
        ctx.SubmitUpdates()

    let DeleteTask id = 
        let task = findTask id
        task.Delete()
        ctx.SubmitUpdates()

    let UpdateTask id description complete = 
        let task = findTask id
        task.complete <- if complete then 1L else 0L
        task.task <- description
        ctx.SubmitUpdates()
