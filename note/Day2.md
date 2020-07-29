## Visual Studio

## Live example: our first grasshopper plugin
rhino6 选择.NET Framework 4.5
rhino5 选择.NET Framework 4

Info是信息文件
Component是实际的工程文件

## C# class definition "template" for a custom Grasshopper component
```c#
using ...

namespace Workshop
{

    public class MyFirstGrasshopperComponent : GH_Component//类名默认应该是Workshop，但是是可以修改的
    {
        //在这个class中，有五个位置是可以写代码的
        public MyFirstGrasshopperComponent()//core constructor:customize the name of your component and where it appear
            : base("MyComponent","Nickname","Description","Category","Subcategory")
        { }

        protected override void RegisterInputParams(GH_Component,GH_InputParamManager pManager)
        { }//define the input parameter

        protected override void RegisterOutputParams(GH_Component,GH_OutputParamManager pManager)
        { }//define the output parameter

        protected override void SolveInstance(IGH_DataAccess DA)
        { }//where the actual computation logic taking place
    }
}
```

```c#
//具体的例子，对于grasshopper原有的Construct Point(Pt)这个电池来说
public WorkshopComponent()
		  : base("Construct Point",//"Workshop",
				 "Pt",//"Nickname",
			     "Construct a point from (xyz) coordinates",//"Description",
			     "Vector",//"Category",that where you can specify where the components appear, 
				 "Points",//"Subcategory",that is the sub panel within the category)
		{
		}
```

生成解决方案，并将gha文件放入grasshopper的libraries文件夹中

但是我们可以change the output path ,so that the output gha file can be directly to the libraries folder

在解决方案的Properties中的生成事件中的生成后事件命令行
- Copy "$(TargetPath)" "$(TargetDir)$(ProjectName).gha"//rename the file from .dll to .gha
- Erase "$(TargetPath)"

修改替换$(TargetDir)$(ProjectName).gha，将他改为libraries文件夹下文件，
例如修改为C:\Users\52305\AppData\Roaming\Grasshopper\Libraries\Workshop.gha

对于编译生成的gha文件的实时更新：

1. 在rhino中输入grasshopper developer setting 然后确保Assembly Loading的勾是勾选的
2. 然后关闭当前的grasshopper文件，再在rhino命令行中输入GrasshopperReloadAssemblies（注意这是一个隐藏命令，没有自动填充）

## The "Average" component - the constructor
```c#
    public GhcAverage()
		  : base("Average of 2 numbers",
				 "Avrg",
			     "Compute the average of 2 numbers",
			     "Workshop",
				 "Utilities")
		{
		}
```

```c#
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("First Number", "First", "The first number", GH_ParamAccess.item, 0.0);
			pManager.AddNumberParameter("Second Number", "Second", "The second number", GH_ParamAccess.item, 0.0);
		}

    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddNumberParameter("Average Value", "Average", "The average value", GH_ParamAccess.item);
		}
```
## The "Average" component - the main part
```c#
    protected override void SolveInstance(IGH_DataAccess DA)
		{
			double a = double.NaN;//declare C# varible of type double ,and a will hold the actual value that sent in from the input part
			//and I initialize two varibles
			//we give a a default value you can set 0.0,but here I give it a NaN,means not a number, a invalid mathematcial value
			//局部变量必须要赋初值
			double b = double.NaN;

			DA.GetData(0, ref a);//GetData is a function at harvest the actual data from an input part
			//the first parameter is the index of the input part, and the value will be instore a variable a.
			//使用ref是为了让a的变化，能够在GetData这个函数运行结束后，仍然保留，这也是GetData这一步的目的所在
			DA.GetData(1, ref b);

			double average = 0.5 * (a + b);

			DA.SetData(0, average);//send the value of average to the output part
		}
```

## Grasshopper icon design tips

### icon for component
icon：
- 分辨率：24*24
- DPI：96pixel/inch（非常重要，否则icon会不清楚）
- 格式：png，svg 不要使用jpeg，jepg首先没有透明度，其次会边缘锯齿模糊

visual studio设置：
- properties中，打开资源，然后添加，它会自动create a new resource file
- 再点击添加资源，可以导入外部的，也可以自己画

最后,在这部分代码中，return部分输入Properties.Resources.文件名
```c#
    protected override System.Drawing.Bitmap Icon
		{
			get
			{
				//You can add image files to your project resources and access them like this:
				// return Resources.IconForThisComponent;
				return Properties.Resources.AverageIcon;
			}
		}
```

### icon for plugin
到 Info.cs 这个文件中，这个文件是all of the basic info about your plugin

同样找到icon function
```c#
    public override Bitmap Icon
		{
			get
			{
				//Return a 24x24 pixel bitmap to represent this GHA library.
				return Properties.Resources.AverageIcon;
			}
		}
```
## The "Average" component - check for input validity
```c#
protected override void SolveInstance(IGH_DataAccess DA)
		{
			double a = double.NaN;
			double b = double.NaN;

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
```
## Inputs and outputs as **lists**

### Live example: A component that computes the centroid of a set of points, and the distance from the centroid to each point

```c#
	protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> iPoints =  new List<Point3d>();
            DA.GetDataList("Points", iPoints);//第一个参数可以用输入时的name或者是输入时的序号

            Point3d centroid = new Point3d(0.0,0.0,0.0);
            foreach (Point3d point  in iPoints)
            {
                centroid += point;
            }
            centroid /= iPoints.Count;
            DA.SetData("Centroid", centroid);

            List<double> distances = new List<double>();
            foreach (Point3d point in iPoints)
            {
                distances.Add(centroid.DistanceTo(point));
            }
            DA.SetDataList("Distance", distances);
        }
```

## Moving Particle

## Objected-Oriented Programming
class = user-defined data type

### Defining a class

#### use grasshopper C# script

把这部分代码放进// <Custom additional code>中
```c#
class Pyramid
{
    public Plane BasePlane;
    public double Lenght;
    public double Width;
    public double Height;

    //构造函数 - 声明并实现
    public Pyramid(Plane basePlane, double length, double width, double height)
    {
        //similar to the __init__ function in python
        BasePlane = basePlane;
        Length = length;
        Width = width;
        Height = height;
    }
    //重载构造函数
    public Pyramid(double length, double width, double height)
    {
        BasePlane = Plane.WorldXY;
        Length = length;
        Width = width;
        Height = height;
    }
    //默认构造函数
    public Pyramid()
    {
        BasePlane = Plane.WorldXY;
        Length = 1.0;
        Width = 1.0;
        Height = 1.0;
    }

    public double ComputeVolume()
    {
        return 0.3333 * Length * Width * Height;
    }
}
```
### Using the class(client code)
#### 把这部分代码放进private void RunScript(object x, object y, ref object A)内
```c#
// Pyramid myPyramid = new Pyramid();

// myPyramid.BasePlane = Plane.WorldXY;
// myPyramid.Length = 1.2;
// myPyramid.Wideth = 3.4;
// myPyramid.Height = 5.6;

Pyramid myPyramid = new Pyramid(Plane.WorldXY,1.2,3.4,5.6);
Pyramid yourPyramid = new Pyramid(3.4,5.6,1.2);
Pyramdi hisPyramid = new Pyramid();

Print(myPyramid.ComputeVolume().ToString());
```