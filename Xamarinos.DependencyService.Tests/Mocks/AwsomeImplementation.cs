using System;
using DependencyServiceUnitTests;

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

