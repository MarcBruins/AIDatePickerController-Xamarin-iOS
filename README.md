Port of https://github.com/alikaragoz/AIDatePickerController

Credits go to Ali Karagoz(https://twitter.com/alikaragoz)

*Including a sample project on how to set this up in Xamarin-iOS with DateTime instead of NSDate.

AIDatePickerController
--------------------

<p align="center"><img src="https://github.com/alikaragoz/AIDatePickerController/blob/master/github-assets/aidatepickercontroller.gif"/></p>

##Usage

```objc
var picker = new AIDatePickerController(DateTime.Now.AddDays(1),
               (p) => 
			{
				Console.WriteLine(p.DatePicker.Date.ToString());
			},
               (p) =>
			{
			this.DismissViewController(true,null);
			 });


		
// Present it
btn.TouchUpInside += (sender, e) =>
{
	this.PresentViewController(picker, true, null);
};
```

## Contact

Ali Karagoz
- http://www.marcbruins.nl
- https://twitter.com/MarcBruins

## License

AIDatePickerController is available under the MIT license. See the LICENSE file for more info.
