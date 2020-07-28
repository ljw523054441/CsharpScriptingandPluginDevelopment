using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Workshop
{
	public class GhcAverage : GH_Component
	{
		/// <summary>
		/// Initializes a new instance of the GhcAverage class.
		/// </summary>
		public GhcAverage()
		  : base("Average of 2 numbers",
				 "Avrg",
			     "Compute the average of 2 numbers",
			     "Workshop",
				 "Utilities")
		{
		}

		/// <summary>
		/// Registers all the input parameters for this component.
		/// </summary>
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("First Number", "First", "The first number", GH_ParamAccess.item, 0.0);
			pManager.AddNumberParameter("Second Number", "Second", "The second number", GH_ParamAccess.item, 0.0);
		}

		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddNumberParameter("Average Value", "Average", "The average value", GH_ParamAccess.item);
		}

		/// <summary>
		/// This is the method that actually does the work.
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double a = double.NaN;//declare C# varible of type double ,and a will hold the actual value that sent in from the input part
			//and I initialize two varibles
			//we give a a default value you can set 0.0,but here I give it a NaN,means not a number, a invalid mathematcial value
			//局部变量必须要赋初值
			double b = double.NaN;

			//DA.GetData(0, ref a);//GetData is a function at harvest the actual data from an input part
			//the first parameter is the index of the input part, and the value will be instore a variable a.
			//使用ref是为了让a的变化，能够在GetData这个函数运行结束后，仍然保留，这也是GetData这一步的目的所在
			//DA.GetData(1, ref b);

			bool success1 = DA.GetData(0, ref a);
			bool success2 = DA.GetData(1, ref b);

			if (success1 && success2)
			{
				double average = 0.5 * (a + b);
				
				DA.SetData(0, average);//send the value of average to the output part
			}
            else
            {
				AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Check the inputs, you idiot!!!");
				//显示电池运行错误时候出现的红色小气球，Error表示错误会显示红色，Warning表示警告会显示橙黄色
            }
		}

		/// <summary>
		/// Provides an Icon for the component.
		/// </summary>
		protected override System.Drawing.Bitmap Icon
		{
			get
			{
				//You can add image files to your project resources and access them like this:
				// return Resources.IconForThisComponent;
				return Properties.Resources.AverageIcon;
			}
		}

		/// <summary>
		/// Gets the unique ID for this component. Do not change this ID after release.
		/// </summary>
		public override Guid ComponentGuid
		{
			get { return new Guid("874c4183-869a-4f3e-95b9-6b1ffbb8b703"); }
		}
	}
}