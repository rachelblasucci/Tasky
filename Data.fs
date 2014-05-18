namespace Tasky 
open FSharp.Data.Sql
open System
open System.IO
open System.Linq

module Data = 

    type sql = SqlDataProvider<ConnectionString = @"Data Source=/Users/rachel/Dropbox/Code/Github/Tasky/Resources/task.sqlite;Version=3;",
                               DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
                               ResolutionPath = @"/Library/Frameworks/Mono.framework/Libraries/mono/4.5/",
                               UseOptionTypes = false>

    type task = { Description : string; Complete : int64; }

    let ctx = sql.GetDataContext()

    let GetIncompleteTasks() = 
        ctx.``[main].[tasks]``
            |> Seq.filter (fun t -> t.complete = 0L)
            |> Seq.map (fun t -> {Description=t.task; Complete=t.complete})
            |> Seq.toArray

//        let tasks = query { for data in ctx.``[main].[tasks]`` do 
//                            where (data.complete = 0L)
//                            select (data.task, data.complete) }
//                        |> Seq.toArray
//        tasks |> Array.map (fun (t, u) -> {Description=t; Complete=u})

    let AddTask description = 
        let newTask = ctx.``[main].[tasks]``.Create()
        newTask.task <- description
        ctx.``Submit Updates``()
