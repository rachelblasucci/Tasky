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

        let addView = new UIView(this.View.Bounds, BackgroundColor = UIColor.White)

        let description = new UITextField(RectangleF(20.f, 64.f, 280.f, 50.f),
                                          Text = task.Description,
                                          Placeholder = "Task description",
                                          ClearButtonMode = UITextFieldViewMode.WhileEditing)
        addView.Add description

        let completeLabel = new UILabel(RectangleF(20.f, 114.f, 100.f, 30.f), Text = "Complete ")
        addView.Add completeLabel

        let completeCheck = new UISwitch(RectangleF(120.f, 114.f, 200.f, 30.f))
        completeCheck.SetState(task.Complete,false)

        completeCheck.TouchDragInside.AddHandler (fun sender eventargs -> task.Complete <- completeCheck.On)
        addView.Add completeCheck

        let addedLabel = new UILabel(RectangleF(20.f, 214.f, 280.f, 50.f),
                                     TextAlignment = UITextAlignment.Center)
        addView.Add addedLabel

        let addUpdateButton = UIButton.FromType(UIButtonType.RoundedRect, Frame = RectangleF(20.f, 164.f, 280.f, 50.f))

        addUpdateButton.TouchUpInside.AddHandler
              (fun _ _ -> 
                let taskDescription =
                   match description.Text.Trim() with
                   | "" -> "New task"
                   | _ -> description.Text

                match isNew with 
                | true -> 
                    Data.AddTask taskDescription
                    addedLabel.Text <- "Added!"
                | false -> 
                    Data.UpdateTask taskDescription completeCheck.On
                    addedLabel.Text <- "Updated!"

                description.Text <- "")

        addUpdateButton.SetTitle("Save", UIControlState.Normal)
        addView.Add addUpdateButton

        description.TouchDown.AddHandler (fun _ _ -> addedLabel.Text <- "")

        this.View.Add addView
       