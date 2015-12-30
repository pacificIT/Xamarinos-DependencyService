# *Xamarinos.DependencyService*

Xamarinos.DependencyService is an alternate dependency service for Xamarin Forms.
It come to add support for dependencies arrays (several dependencies implementing the same interface).
It can be used in combination with the standard xamarin dependency service as shown below :

### Setup

Reference Xamarinos.DependencyService shared project within your solution.

### Usage

```c#
var arrayOfITested = Xamarinos.DependencyService.GetAll<ITested> ();

foreach (var implementation in arrayOfITested) 
{
  implementation.DoSomething();
}
```

### Dependency registering

```c#
[assembly: Xamarinos.Dependency (typeof (AwsomeImplementation))]
[assembly: Xamarin.Forms.Dependency (typeof (AwsomeImplementation))]
namespace DependencyServiceUnitTests
{
	public class AwsomeImplementation : ITested
	{
		public AwsomeImplementation ()
		{
		}

		#region ITested implementation

		public int UniqueId 
		{
			get { return 42; }
		}

		#endregion
	}
}
```

Check the unit tests to get more examples !
