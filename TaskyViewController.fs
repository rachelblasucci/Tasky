namespace Tasky

open MonoTouch.UIKit
open MonoTouch.Foundation
open System
open System.IO
open Data

type TaskDataSource(tasks: task list, navigation: UINavigationController) = 
    inherit UITableViewSource()
    member x.cellIdentifier = "TaskCell"
    override x.RowsInSection(view, section) = tasks.Length
    override x.CanEditRow (view, indexPath) = true
    override x.GetCell(view, indexPath) = 
        let t = tasks.[indexPath.Item]
        let cell =
            let newCell = view.DequeueReusableCell x.cellIdentifier 
            match newCell with 
                | null -> new UITableViewCell(UITableViewCellStyle.Default, x.cellIdentifier)
                | _ -> newCell
        cell.TextLabel.Text <- t.Description
        cell
    override x.RowSelected (tableView, indexPath) = 
        tableView.DeselectRow (indexPath, false)
        navigation.PushViewController (new AddTaskViewController(tasks.[indexPath.Item], false), true)
    override x.CommitEditingStyle(view, editingStyle, indexPath) = 
        match editingStyle with 
            | UITableViewCellEditingStyle.Delete -> 
                Data.DeleteTask tasks.[indexPath.Item].Description
                view.DeleteRows([|indexPath|], UITableViewRowAnimation.Fade)
            | _ -> Console.WriteLine "CommitEditingStyle:None called"

[<Register ("TaskyViewController")>]
type TaskyViewController () as this =
    inherit UIViewController ()

    let table = new UITableView()

    let addNewTask = 
        new EventHandler(fun sender eventargs -> 
            this.NavigationController.PushViewController (new AddTaskViewController(), true)
        )

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem(UIBarButtonSystemItem.Add, addNewTask), false)
        table.Frame <- this.View.Bounds
        this.View.Add table 

    override this.ViewWillAppear animated =
        base.ViewWillAppear animated
        table.Source <- new TaskDataSource(Data.GetIncompleteTasks (), this.NavigationController)
        table.ReloadData()
    
