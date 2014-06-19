namespace Tasky

open System
open MonoTouch.UIKit
open MonoTouch.Foundation
open System.Drawing
open Data

type AddTaskViewController (task:task, isNew:bool) =
    inherit UIViewController ()
    new() = new AddTaskViewController ({Description=""; Complete=false}, true)
    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        let addView = new UIView(this.View.Bounds)
        addView.BackgroundColor <- UIColor.White

        let description = new UITextField(new RectangleF(20.f, 64.f, 280.f, 50.f))
        description.Text <- task.Description
        description.Placeholder <- "task description"

        let completeLabel = new UILabel(new RectangleF(20.f, 114.f, 100.f, 30.f))
        completeLabel.Text <- "Complete "
        addView.Add completeLabel

        let completeCheck = new UISwitch(new RectangleF(120.f, 114.f, 200.f, 30.f))
        completeCheck.SetState(task.Complete,false)
        let changeCompleteStatus = 
            new EventHandler (fun sender eventargs ->
                task.Complete <- completeCheck.On
            ) 
        completeCheck.TouchDragInside.AddHandler changeCompleteStatus
        addView.Add completeCheck

        let addedLabel = new UILabel(new RectangleF(20.f, 264.f, 200.f, 50.f))
        addView.Add addedLabel

        let clearLabel = 
            new EventHandler(fun sender eventargs -> 
                addedLabel.Text <- ""
            )
        description.TouchDown.AddHandler clearLabel
        addView.Add description

        let addUpdateButton = UIButton.FromType(UIButtonType.RoundedRect)
        addUpdateButton.Frame <- new RectangleF(20.f, 164.f, 280.f, 50.f)

        let addUpdateHandler = 
            new EventHandler(fun sender eventargs -> 
                match isNew with 
                    | true -> 
                        Data.AddTask description.Text
                        addedLabel.Text <- "Added!"
                    | false -> 
                        Data.UpdateTask description.Text completeCheck.On
                        addedLabel.Text <- "Updated!"
                description.Text <- ""
            )

        addUpdateButton.TouchUpInside.AddHandler addUpdateHandler
        let buttonTitle = "Save"
        addUpdateButton.SetTitle(buttonTitle, UIControlState.Normal)
        addView.Add addUpdateButton

        if isNew then
            addedLabel.Frame <- new RectangleF(20.f, 214.f, 280.f, 50.f)
        else
            let deleteButton = UIButton.FromType(UIButtonType.RoundedRect)
            deleteButton.Frame <- new RectangleF(20.f, 214.f, 280.f, 50.f)

            let deleteHandler = 
                new EventHandler(fun sender eventargs -> 
                    Data.DeleteTask description.Text
                    addedLabel.Text <- "Deleted!"
                    description.Text <- ""
                )

            deleteButton.TouchUpInside.AddHandler deleteHandler
            deleteButton.SetTitle("Delete task", UIControlState.Normal)
            addView.Add deleteButton

        this.View.Add addView
       