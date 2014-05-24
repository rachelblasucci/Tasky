namespace Tasky

open System
open MonoTouch.UIKit
open MonoTouch.Foundation
open System.Drawing
open Data

[<Register ("AddTaskViewController")>]
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

        let addedLabel = new UILabel(new RectangleF(20.f, 214.f, 200.f, 50.f))
        addView.Add addedLabel

        let clearLabel = 
            new EventHandler(fun sender eventargs -> 
                addedLabel.Text <- ""
            )
        description.TouchDown.AddHandler clearLabel
        addView.Add description

        let button = UIButton.FromType(UIButtonType.RoundedRect)
        button.Frame <- new RectangleF(20.f, 164.f, 280.f, 50.f)

        let buttonHandler = 
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

        button.TouchUpInside.AddHandler buttonHandler
        let buttonTitle = match isNew with 
                            | true -> "Add task"
                            | false -> "Update task"
        button.SetTitle(buttonTitle, UIControlState.Normal)
        addView.Add button

        this.View.Add addView
       