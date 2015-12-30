using System;

namespace Xamarinos
{
	[AttributeUsage (AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public class DependencyAttribute : Attribute
	{
		//
		// Properties
		//
		public Type Implementor {
			get;
			private set;
		}

		//
		// Constructors
		//
		public DependencyAttribute (Type implementorType)
		{
			this.Implementor = implementorType;
		}
	}
}

