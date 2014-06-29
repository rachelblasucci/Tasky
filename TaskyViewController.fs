namespace Tasky

open MonoTouch.UIKit
open MonoTouch.Foundation
open System
open System.Collections.Generic
open System.IO
open Data

type TaskDataSource(tasksource: task list, navigation: UINavigationController) = 
    inherit UITableViewSource()
    let tasks = new List<task>(tasksource)
    member x.cellIdentifier = "TaskCell"
    override x.RowsInSection(view, section) = tasks.Count
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
    override x.CanEditRow (view, indexPath) = true
    override x.CommitEditingStyle(view, editingStyle, indexPath) = 
        match editingStyle with 
            | UITableViewCellEditingStyle.Delete ->
                Data.DeleteTask tasks.[indexPath.Item].Description
                tasks.RemoveAt(indexPath.Item)
                view.DeleteRows([|indexPath|], UITableViewRowAnimation.Fade)
            | _ -> Console.WriteLine "CommitEditingStyle:None called"


type TaskyViewController () =
    inherit UIViewController ()

    let table = new UITableView()

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        let addNewTask = 
            new EventHandler(fun sender eventargs -> 
                this.NavigationController.PushViewController (new AddTaskViewController(), true)
            )
        this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem(UIBarButtonSystemItem.Add, addNewTask), false)
        table.Frame <- this.View.Bounds
        this.View.Add table 

    override this.ViewWillAppear animated =
        base.ViewWillAppear animated
        table.Source <- new TaskDataSource(Data.GetIncompleteTasks(), this.NavigationController)
        table.ReloadData()
