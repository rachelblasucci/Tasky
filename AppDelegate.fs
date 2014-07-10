namespace Tasky

open System
open MonoTouch.UIKit
open MonoTouch.Foundation

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit UIApplicationDelegate ()

    override val Window = null with get,set

    // This method is invoked when the application is ready to run.
    override this.FinishedLaunching (app, options) =
        // If you have defined a root view controller, set it here:
        this.Window <- new UIWindow (UIScreen.MainScreen.Bounds)
        this.Window.RootViewController <- new UINavigationController(new TaskyViewController ())
        this.Window.MakeKeyAndVisible ()
        true

module Main =
    [<EntryPoint>]
    let main args =
        UIApplication.Main (args, null, "AppDelegate")
        0

