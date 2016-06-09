using System;
using Foundation;
using UIKit;

namespace DatePicker
{
	public partial class AIDatePickerController : UIViewController, IUIViewControllerAnimatedTransitioning, IUIViewControllerTransitioningDelegate
	{
		public double AnimatedTransitionDuration
		{
			get;
			set;
		} = 0.4;

		public UIDatePicker DatePicker;

		public UIButton CancelButton;
		public UIButton SelectButton;
		public UIButton DismissButton;
		public UIView ButtonDividerView;
		public UIView ButtonContainerView;
		public UIView DatePickerContainerView;
		public UIView DimmedView;

		public Action<AIDatePickerController> SelectAction;
		public Action<AIDatePickerController> CancelAction;


		private UIColor datePickerBackgroundColor = UIColor.White;
		public UIColor DatePickerBackgroundColor
		{
			get { return datePickerBackgroundColor; }
			set { datePickerBackgroundColor = value; UpdateUI(); }
		}

		private UIDatePickerMode datePickerMode = UIDatePickerMode.Date;
		public UIDatePickerMode DatePickerMode
		{
			get { return datePickerMode; }
			set { datePickerMode = value; UpdateUI(); }
		}

		private float fontSize = 16;
		public float FontSize
		{
			get { return fontSize; }
			set { fontSize = value; UpdateUI(); }
		}

		public NSDateFormatter dateFormatter
		{
			get;
			set;
		} = new NSDateFormatter();

		public AIDatePickerController(DateTime initialDateTime, Action<AIDatePickerController> selectAction, Action<AIDatePickerController> cancelAction) : base()
		{
			this.SelectAction = selectAction;
			this.CancelAction = cancelAction;

			this.ModalPresentationStyle = UIModalPresentationStyle.Custom;
			this.TransitioningDelegate = this;

			// Date Picker
			DatePicker = new UIDatePicker();
			DatePicker.TranslatesAutoresizingMaskIntoConstraints = false;
			DatePicker.Date = initialDateTime.DateTimeToNSDate();

			DimmedView = new UIView(this.View.Bounds);
			DimmedView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			DimmedView.TintAdjustmentMode = UIViewTintAdjustmentMode.Dimmed;
			DimmedView.BackgroundColor = UIColor.Black;
		}

		private void UpdateUI()
		{
			DatePicker.BackgroundColor = DatePickerBackgroundColor;
			DatePicker.Mode = DatePickerMode;

			SelectButton.TitleLabel.Font = UIFont.BoldSystemFontOfSize(FontSize);
			CancelButton.TitleLabel.Font = UIFont.SystemFontOfSize(FontSize);
		}

		public override void LoadView()
		{
			base.LoadView();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.View.BackgroundColor = UIColor.Clear;

			DismissButton = new UIButton();
			DismissButton.TranslatesAutoresizingMaskIntoConstraints = false;
			DismissButton.UserInteractionEnabled = true;
			DismissButton.TouchUpInside += (s, e) =>
			{
				CancelAction(this);
			};
			this.View.AddSubview(DismissButton);

			DatePickerContainerView = new UIView();
			DatePickerContainerView.TranslatesAutoresizingMaskIntoConstraints = false;
			DatePickerContainerView.BackgroundColor = UIColor.White;
			DatePickerContainerView.ClipsToBounds = true;
			DatePickerContainerView.Layer.CornerRadius = 5.0f;
			this.View.AddSubview(DatePickerContainerView);

			DatePickerContainerView.AddSubview(DatePicker);

			ButtonContainerView = new UIView();
			ButtonContainerView.TranslatesAutoresizingMaskIntoConstraints = false;
			ButtonContainerView.BackgroundColor = UIColor.White;
			ButtonContainerView.Layer.CornerRadius = 5.0f;
			this.View.AddSubview(ButtonContainerView);

			ButtonDividerView = new UIView();
			ButtonDividerView.TranslatesAutoresizingMaskIntoConstraints = false;
			ButtonDividerView.BackgroundColor = UIColor.FromRGBA(205 / 255, 205 / 255, 205 / 255, 1);
			this.View.AddSubview(ButtonDividerView);

			CancelButton = new UIButton();
			CancelButton.TranslatesAutoresizingMaskIntoConstraints = false;
			CancelButton.SetTitle("Cancel", UIControlState.Normal);
			CancelButton.SetTitleColor(UIColor.Red, UIControlState.Normal);
			CancelButton.TouchUpInside += (s, e) =>
			{
				CancelAction(this);
			};
			ButtonContainerView.AddSubview(CancelButton);

			SelectButton = new UIButton(UIButtonType.System);
			SelectButton.TranslatesAutoresizingMaskIntoConstraints = false;
			SelectButton.SetTitle("Select", UIControlState.Normal);
			SelectButton.TouchUpInside += (s, e) =>
			{
				SelectAction(this);
			};
			ButtonContainerView.AddSubview(SelectButton);

			var views = NSDictionary.FromObjectsAndKeys(
				new NSObject[] { DismissButton, DatePickerContainerView, DatePicker, ButtonContainerView, ButtonDividerView, CancelButton, SelectButton },
				new NSObject[] {
					new NSString("DismissButton"), new NSString("DatePickerContainerView"), new NSString("datePicker"),
					new NSString("ButtonContainerView"), new NSString("ButtonDividerView"), new NSString("CancelButton"),
					new NSString("SelectButton")
				}
			);

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[CancelButton][ButtonDividerView(0.5)][SelectButton(CancelButton)]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[CancelButton]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[ButtonDividerView]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[SelectButton]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[datePicker]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[datePicker]|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[DismissButton]|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-5-[DatePickerContainerView]-5-|", 0, null, views));
			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|-5-[ButtonContainerView]-5-|", 0, null, views));

			this.View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[DismissButton][DatePickerContainerView]-10-[ButtonContainerView(40)]-5-|", 0, null, views));

			UpdateUI();
		}

		public double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
		{
			return AnimatedTransitionDuration;
		}

		public void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
		{
			var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
			var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
			var containerView = transitionContext.ContainerView;

			//if we are presenting
			if (toViewController.View == this.View)
			{
				fromViewController.View.UserInteractionEnabled = false;

				containerView.AddSubview(DimmedView);
				containerView.AddSubview(toViewController.View);

				var frame = toViewController.View.Frame;
				frame.Y = toViewController.View.Bounds.Height;
				toViewController.View.Frame = frame;

				this.DimmedView.Alpha = 0f;

				UIView.Animate(AnimatedTransitionDuration, 0, UIViewAnimationOptions.CurveEaseIn,
				   () =>
					{
						this.DimmedView.Alpha = 0.5f;
						frame = toViewController.View.Frame;
						frame.Y = 0f;
						toViewController.View.Frame = frame;
					},
				   () =>
					{
						transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
					}
				  );

			}
			else
			{
				toViewController.View.UserInteractionEnabled = true;
				UIView.Animate(AnimatedTransitionDuration, 0.1f, UIViewAnimationOptions.CurveEaseIn,
				   () =>
					{
						this.DimmedView.Alpha = 0f;
						var frame = fromViewController.View.Frame;
						frame.Y = fromViewController.View.Bounds.Height;
						fromViewController.View.Frame = frame;
					},
				   () =>
					{
						transitionContext.CompleteTransition(!transitionContext.TransitionWasCancelled);
					}
				  );
			}
		}

		[Export("animationControllerForPresentedController:presentingController:sourceController:")]
		public IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
		{
			return this;
		}

		[Export("animationControllerForDismissedController:")]
		public IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
		{
			return this;
		}
	}
}


