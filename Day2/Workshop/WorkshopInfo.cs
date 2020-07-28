using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Workshop
{
	public class WorkshopInfo : GH_AssemblyInfo
	{
		public override string Name
		{
			get
			{
				return "Workshop";
			}
		}
		public override Bitmap Icon
		{
			get
			{
				//Return a 24x24 pixel bitmap to represent this GHA library.
				return Properties.Resources.AverageIcon;
			}
		}
		public override string Description
		{
			get
			{
				//Return a short string describing the purpose of this GHA library.
				return "";
			}
		}
		public override Guid Id
		{
			get
			{
				return new Guid("9de18b16-ae9f-4aa4-a15e-ae1651e9f7a3");
			}
		}

		public override string AuthorName
		{
			get
			{
				//Return a string identifying you or your company.
				return "";
			}
		}
		public override string AuthorContact
		{
			get
			{
				//Return a string representing your preferred contact details.
				return "";
			}
		}
	}
}
