using System;
using AI;
using UIKit;

namespace sample
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
		}


        public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            var picker = new AIDatePickerController
            {
                Mode = UIDatePickerMode.Date,
                SelectedDateTime = DateTime.Now.AddDays(1),
                MinuteInterval = 1,
                MinimumDateTime = DateTime.Now.AddDays(-7),
                MaximumDateTime = DateTime.Now.AddDays(7),
                OkText = "OK",
                CancelText = "GO AWAY",
                Ok = x => OnAction("OK", x.SelectedDateTime),
                Cancel = x => OnAction("CANCEL", x.SelectedDateTime)
            };

			btn.TouchUpInside += (sender, e) =>
			{
				this.PresentViewController(picker, true, null);
			};
		}


	    void OnAction(string action, DateTime dt)
	    {
	        var alert = UIAlertController.Create("Hi", $"{action} - {dt}", UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(alert, true, null);
	    }
	}
}

