namespace Tasky

open System
open MonoTouch.UIKit
open MonoTouch.Foundation
open System.Drawing
open Data

[<Register ("AddTaskViewController")>]
type AddTaskViewController () =
    inherit UIViewController ()

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        let addView = new UIView(this.View.Bounds)
        addView.BackgroundColor <- UIColor.White

        let label = new UILabel(new RectangleF(20.f, 164.f, 200.f, 50.f))
        addView.Add label

        let description = new UITextField(new RectangleF(20.f, 64.f, 280.f, 50.f))
        description.Placeholder <- "task description"
        let clearLabel = 
            new EventHandler(fun sender eventargs -> 
                label.Text <- ""
            )
        description.TouchDown.AddHandler clearLabel
        addView.Add description

        let button = UIButton.FromType(UIButtonType.RoundedRect)
        button.Frame <- new RectangleF(20.f, 114.f, 65.f, 50.f)
        let submitTask = 
            new EventHandler(fun sender eventargs -> 
                Data.AddTask description.Text
                description.Text <- ""
                label.Text <- "Added!"
            )
        button.TouchUpInside.AddHandler submitTask
        button.SetTitle("Add task", UIControlState.Normal)
        addView.Add button

        this.View.Add addView
       