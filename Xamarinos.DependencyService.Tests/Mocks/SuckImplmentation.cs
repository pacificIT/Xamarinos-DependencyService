using System;
using DependencyServiceUnitTests;

[assembly: Xamarinos.Dependency (typeof (SuckImplmentation))]
[assembly: Xamarin.Forms.Dependency (typeof (SuckImplmentation))]
namespace DependencyServiceUnitTests
{
	public class SuckImplmentation : ITested
	{
		public SuckImplmentation ()
		{
		}

		#region ITested implementation

		public int UniqueId 
		{
			get { return 0; }
		}

		#endregion
	}
}

