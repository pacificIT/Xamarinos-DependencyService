using System;
using System.Linq;
using NUnit.Framework;

namespace DependencyServiceUnitTests
{
	[TestFixture]
	public class DependencyServiceTest
	{
		[SetUp]
		public void Setup()
		{
		}

		[TearDown]
		public void Tear ()
		{
		}

		[Test]
		public void TestSingleGlobal()
		{
			var xamarinDependencyService = Xamarin.Forms.DependencyService.Get<ITested> ();
			var xamarinosDependencyService = Xamarinos.DependencyService.Get<ITested> ();

			Assert.IsNotNull (xamarinDependencyService);
			Assert.IsNotNull (xamarinosDependencyService);
			Assert.AreEqual (xamarinDependencyService.GetType(), xamarinosDependencyService.GetType());
			Assert.AreEqual (xamarinDependencyService.UniqueId, xamarinosDependencyService.UniqueId);

			var xamarinosDependencyService2 = Xamarinos.DependencyService.Get<ITested> ();
			Assert.AreSame (xamarinosDependencyService2, xamarinosDependencyService);
		}

		[Test]
		public void TestSingleNotGlobal()
		{
			var xamarinDependencyService = Xamarin.Forms.DependencyService.Get<ITested> ();
			var xamarinosDependencyService = Xamarinos.DependencyService.Get<ITested> ();

			Assert.IsNotNull (xamarinDependencyService);
			Assert.IsNotNull (xamarinosDependencyService);
			Assert.AreEqual (xamarinDependencyService.GetType(), xamarinosDependencyService.GetType());
			Assert.AreEqual (xamarinDependencyService.UniqueId, xamarinosDependencyService.UniqueId);

			var xamarinosDependencyService2 = Xamarinos.DependencyService.Get<ITested> (Xamarinos.DependencyFetchTarget.NewInstance);
			Assert.AreNotSame (xamarinosDependencyService2, xamarinosDependencyService);
		}

		[Test]
		public void TestMultipleGlobal()
		{
			var xamarinosDependencyService = Xamarinos.DependencyService.GetAll<ITested> ();

			Assert.IsNotNull (xamarinosDependencyService);
			Assert.IsTrue (xamarinosDependencyService.Count() > 1);
			Assert.AreEqual (xamarinosDependencyService.Count(), xamarinosDependencyService.Where(x => x is ITested).Count());

			var xamarinosDependencyService2 = Xamarinos.DependencyService.GetAll<ITested> ();
			for (int i = 0; i < xamarinosDependencyService.Count (); i++) 
			{
				Assert.AreSame (xamarinosDependencyService [i], xamarinosDependencyService2 [i]);
			}
		}

		[Test]
		public void TestMultipleNotGlobal()
		{
			var xamarinosDependencyService = Xamarinos.DependencyService.GetAll<ITested> ();

			Assert.IsNotNull (xamarinosDependencyService);
			Assert.IsTrue (xamarinosDependencyService.Count() > 1);
			Assert.AreEqual (xamarinosDependencyService.Count(), xamarinosDependencyService.Where(x => x is ITested).Count());

			var xamarinosDependencyService2 = Xamarinos.DependencyService.GetAll<ITested> (Xamarinos.DependencyFetchTarget.NewInstance);
			for (int i = 0; i < xamarinosDependencyService.Count (); i++) 
			{
				Assert.AreNotSame (xamarinosDependencyService [i], xamarinosDependencyService2 [i]);
			}
		}
	}
}

