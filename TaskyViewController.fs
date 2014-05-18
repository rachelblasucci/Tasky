namespace Tasky

open MonoTouch.UIKit
open MonoTouch.Foundation
open System
open System.IO
open Data

type TaskDataSource(tasks: task[]) = 
    inherit UITableViewDataSource()
    member x.cellIdentifier = "TaskCell"
    override x.RowsInSection(view, section) = tasks.Length
    override x.GetCell(view, indexPath) = 
        let t = tasks.[indexPath.Item]
        let cell =
            let newCell = view.DequeueReusableCell x.cellIdentifier 
            match newCell with 
                | null -> new UITableViewCell(UITableViewCellStyle.Default, x.cellIdentifier)
                | _ -> newCell
        cell.TextLabel.Text <- t.Description
        cell

[<Register ("TaskyViewController")>]
type TaskyViewController () as this =
    inherit UIViewController ()

    let table = new UITableView()

    let addNewTask = 
        new EventHandler(fun sender eventargs -> 
            this.NavigationController.PushViewController <| (new AddTaskViewController(), false)
        )

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem(UIBarButtonSystemItem.Add, addNewTask), false)
        table.Frame <- this.View.Bounds
        table.DataSource <- new TaskDataSource(Data.GetIncompleteTasks())
        this.View.Add table 

    override this.ViewWillAppear animated =
        base.ViewWillAppear animated
        table.DataSource <- new TaskDataSource(Data.GetIncompleteTasks())
        table.ReloadData()