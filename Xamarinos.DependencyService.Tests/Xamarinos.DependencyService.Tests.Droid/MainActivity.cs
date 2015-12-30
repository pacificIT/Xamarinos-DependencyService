using System.Reflection;

using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;

namespace Xamarinos.DependencyService.Tests.Droid
{
	[Activity (Label = "Xamarinos.DependencyService.Tests.Droid", MainLauncher = true)]
	public class MainActivity : TestSuiteActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Xamarin.Forms.Forms.Init (this, bundle);

			// tests can be inside the main assembly
			AddTest (Assembly.GetExecutingAssembly ());

			// or in any reference assemblies
			// AddTest (typeof (Your.Library.TestClass).Assembly);

			// Once you called base.OnCreate(), you cannot add more assemblies.
			base.OnCreate (bundle);
		}
	}
}

