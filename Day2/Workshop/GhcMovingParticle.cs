using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Workshop
{
    public class GhcMovingParticle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GhcMovingParticle class.
        /// </summary>
        public GhcMovingParticle()
          : base("Moving Particle", "MvPrtc",
              "Create a moving point in the direction specified by the user",
              "Workshop", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Reset", "Reset", "Reset", GH_ParamAccess.item);
            pManager.AddVectorParameter("Velocity", "Velocity", "Velocity", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Particle", "Particle", "Particle", GH_ParamAccess.item);
        }

        //全局变量在这里声明，不用初始化
        Point3d currentPosition;

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool iReset = true;
            DA.GetData(0, ref iReset);

            Vector3d iVelocity = new Vector3d();
            DA.GetData(1, ref iVelocity);

            if(iReset)
            {
                currentPosition = new Point3d(0, 0, 0);
                return;//return 在这里表示结束当前的SolveInstance函数
            }

            currentPosition += iVelocity;
            DA.SetData(0, currentPosition);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b119cff7-127e-400d-ab28-b79782d0fc74"); }
        }
    }
}