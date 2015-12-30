using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xamarinos
{
	public enum DependencyFetchTarget
	{
		GlobalInstance,
		NewInstance
	}

	public static class DependencyService
	{
		//
		// Static Fields
		//
		private static bool initialized = false;

		private static readonly List<Type> DependencyTypes = new List<Type> ();

		private static readonly Dictionary<Type, DependencyService.DependencyData> DependencyImplementations = new Dictionary<Type, DependencyService.DependencyData> ();

		//
		// Static Methods
		//
		private static IEnumerable<Type> FindImplementors (Type target)
		{
			return DependencyService.DependencyTypes.Where ((Type t) => target.IsAssignableFrom (t));
		}

		public static T Get<T> (DependencyFetchTarget fetchTarget = DependencyFetchTarget.GlobalInstance) where T : class
		{
			if (!DependencyService.initialized) 
			{
				DependencyService.Initialize ();
			}

			Type typeFromHandle = typeof(T);

			if (!DependencyService.DependencyImplementations.ContainsKey (typeFromHandle)) 
			{
				var types = DependencyService.FindImplementors (typeFromHandle);

				DependencyService.DependencyImplementations [typeFromHandle] = ((types != null && types.Count() > 0) 
					? new DependencyService.DependencyData { ImplementorType = types.ToList() } 
					: null);
			} 
				
			DependencyService.DependencyData dependencyData = DependencyService.DependencyImplementations [typeFromHandle];
			if (dependencyData == null) {
				return default(T);
			}

			if (fetchTarget == DependencyFetchTarget.GlobalInstance) 
			{
				if (dependencyData.GlobalInstance.Count() == 0) 
				{
					foreach (var singleImplementorType in dependencyData.ImplementorType) 
					{
						var result = Activator.CreateInstance (singleImplementorType);
						dependencyData.GlobalInstance.Add(result);
					}
				}
				return (T)((object)dependencyData.GlobalInstance.FirstOrDefault());
			}
			return (T)((object)Activator.CreateInstance (dependencyData.ImplementorType.FirstOrDefault()));
		}

		#region Custom dependency array logic

		public static T[] GetAll<T> (DependencyFetchTarget fetchTarget = DependencyFetchTarget.GlobalInstance) where T : class
		{
			return GetAllInternal<T> (fetchTarget).ToArray();
		}

		private static IEnumerable<T> GetAllInternal<T> (DependencyFetchTarget fetchTarget = DependencyFetchTarget.GlobalInstance) where T : class
		{
			if (!DependencyService.initialized) 
			{
				DependencyService.Initialize ();
			}

			Type typeFromHandle = typeof(T);

			if (!DependencyService.DependencyImplementations.ContainsKey (typeFromHandle)) 
			{
				var types = DependencyService.FindImplementors(typeFromHandle);

				DependencyService.DependencyImplementations [typeFromHandle] = ((types != null && types.Count() > 0) 
					? new DependencyService.DependencyData { ImplementorType = types.ToList() } 
					: null);
			} 

			DependencyService.DependencyData dependencyData = DependencyService.DependencyImplementations [typeFromHandle];
			if (dependencyData == null) 
			{
				yield return default(T);
			} 
			else 
			{
				if (fetchTarget == DependencyFetchTarget.GlobalInstance) 
				{
					if (dependencyData.GlobalInstance.Count() == 0) 
					{
						foreach (var singleImplementorType in dependencyData.ImplementorType) 
						{
							var result = Activator.CreateInstance (singleImplementorType);
							dependencyData.GlobalInstance.Add (result);
						}
					}
					foreach (var singleImplementor in dependencyData.GlobalInstance) 
					{
						yield return (T)((object)singleImplementor);
					}
				} 
				else 
				{
					foreach (var singleImplementorType in dependencyData.ImplementorType) 
					{
						yield return (T)((object)Activator.CreateInstance (singleImplementorType));
					}
				}
			}
		}

		#endregion

		private static void Initialize ()
		{
			Assembly[] assemblies = GetAssemblies ();
			Type typeFromHandle = typeof(DependencyAttribute);
			Assembly[] array = assemblies;

			for (int i = 0; i < array.Length; i++) 
			{
				Assembly element = array [i];
				Attribute[] array2 = element.GetCustomAttributes (typeFromHandle).ToArray<Attribute> ();
				if (array2.Length != 0) 
				{
					Attribute[] array3 = array2;
					for (int j = 0; j < array3.Length; j++) 
					{
						DependencyAttribute dependencyAttribute = (DependencyAttribute)array3 [j];

						if (!DependencyService.DependencyTypes.Contains (dependencyAttribute.Implementor)) 
						{
							DependencyService.DependencyTypes.Add (dependencyAttribute.Implementor);
						}
					}
				}
			}
			DependencyService.initialized = true;
		}

		public static Assembly[] GetAssemblies ()
		{
			return AppDomain.CurrentDomain.GetAssemblies ();
		}

//		public static void Register<T> () where T : class
//		{
//			Type typeFromHandle = typeof(T);
//			if (!DependencyService.DependencyTypes.Contains (typeFromHandle)) {
//				DependencyService.DependencyTypes.Add (typeFromHandle);
//			}
//		}
//
//		public static void Register<T, TImpl> () where T : class where TImpl : class, T
//		{
//			Type typeFromHandle = typeof(T);
//			Type typeFromHandle2 = typeof(TImpl);
//
//			if (!DependencyService.DependencyTypes.Contains (typeFromHandle)) {
//				DependencyService.DependencyTypes.Add (typeFromHandle);
//			}
//
//
//			DependencyService.DependencyImplementations [typeFromHandle] = new DependencyService.DependencyData {
//				ImplementorType = typeFromHandle2
//			};
//		}

		//
		// Nested Types
		//
		private class DependencyData
		{
			public DependencyData()
			{
				ImplementorType = new List<Type>();
				GlobalInstance = new List<object>();
			}

			public List<Type> ImplementorType 
			{
				get;
				set;
			}

			public List<object> GlobalInstance 
			{
				get;
				set;
			}
		}
	}
}
