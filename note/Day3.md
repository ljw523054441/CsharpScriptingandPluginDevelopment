## Spatial Data Structure

## namespace
namespace:Classes with identical names can coexist相同名字的类可以共存。可以把namespace理解为一个文件夹，不同文件夹中的相同名字的文件可以共存。

```c#
//RhinoCommon.dll
namespace Rhino.Geometry
{
    public class Curve
    {
        ...
    }
}

//RevitAPI.dll
namespace Autodesk.Revit
{
    public class Curve
    {
        ...
    }
}

//Your GH plugin
namespace Workshop
{
    Rhino.Geometry.Curve myRhinoCurve = new Rhino.Geometry.Curve();//在调用别的命名空间中的类时，要写出全名
    Autodesk.Revit.Curve myRevitCurve = new Autodesk.Revit.Curve();
}

//或者是在最前面，写上using
using Rhino.Geometry;

namespace Workshop
{
    Curve myRhinoCurve = new Curve();
    Autodesk.Revit.Curve myRevitCurve = new Autodesk.Revit.Curve();
}

//注意以下的error情况:Ambiguous!!!
using Rhino.Geometry;
using Autodesk.Revit;

namespace Workshop
{
    Curve myRhinoCurve = new Curve();
    Curve myRevitCurve = new Curve();
}

//为了避免这样的错误发生
using RhinoCurve = Rhino.Geometry.Curve;
using RevitCurve = Autodesk.Revit.Curve;

namespace Workshop
{
    RhinoCurve myRhinoCurve = new RhinoCurve();
    RevitCurve myRevitCurve = new RevitCurve();
}

//命名空间是可以被嵌套使用的（nested）
namespace Rhino.Geometry
{
    public class Curve
    {
        ...
    }
}

namespace Rhino
{
    namespace Geometry
    {
        public class Curve
        {
            ...
        }
    }
}
//以上两种是等价的
//一个命名空间可以包含多个子命名空间
//RhinoCommon.dll
namespace Rhino.Geometry
namespace Rhino.Display
namespace Rhion.Input
```

### Live Example:namespaces and classes  Implementing the Pyramid class in Visual Studio


## Inheritance
```c#
class LandAnimal
{
    public string Name;
    public double Weight;
    protected int legCount;

    protected void eat(){...}
}

class Dog:LandAnimal
{
    public string breed;//这属性只对Dog类有效
    public void bark(){...}
}

class Human:LandAnimal
{
    public string Nationality;
    public void ChangeNationality(){...}
}

//All public and protected (but not private) memebers of LandAnimal will automatically be added to Dog (and Human) class definition

//Inheritance can be indirect and multi-level
class Architect:Human
{
    public string ArchitectLicenseID;
    public void Design(){...}
}

class Engineer:Human
{
    public string EngineerLicenseID;
    public void BuildRobot(){...}
}
```
一些术语（terminology）
- Dog **inherits（继承）** LandAnimal
- Dog is **derived（派生）** from LandAnimal
- LandAnimal is the **parent class(base class)** of Dog
- Dog is a **child class(derived class)** of LandAnimal

### Example:Class inheritance in RhinoCommon

对于Curve类：有Degree,Dimension,Domain等属性；也有ClosestPoint(),PointAt()等方法

LineCurve类，ArcCurve类，NurbsCurve类，PolyCurve类等，继承了Curve类。但是对于LineCurve类，它有自己的属性Line；对于NurbsCurve类，有自己的属性Points，Reparametrize()方法

Curve类继承自GeometryBase类，同样继承于GeometryBase类的还有Brep类，Mesh类，Surface类

GeometrBase类继承自Object类

### Provide a template for class definition
The class definitions for our GH plugin components are based on the GH_Component class "template".
This is how Grasshopper can automatically "recognizes" our components when the .gha file is installed.

```c#
class MyGrasshopperComponent:GH_component
{
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    { }
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    { }
    protected override void SolveInstance(IGH_DataAccess DA)
    { }
}
```

### Polymorphism 多态
provides a common interface for related types.
例如，如果一个函数要求输入一个Curve，我们也可以输入继承自Curve类的其他类型
```c#
//函数定义时
void MoveCurve(Curve curve)
{
    ...
}

//函数调用时
ArcCurve myArcCurve = new ArcCurve(...);
MoveCurve(myArcCurve);

```

### Struct 结构体

## Using External Libraries
### Using a .NET library in the C# Script Component
需要先右键C# Component，选择Manage Assemblies,然后添加.gha文件，随后在C# Component中输入如下
```c#
private MeshGrowthSystem system;

private void RunScript(bool iReset, Mesh iStartingMesh, int iSubiterationCount, bool iGrow, int iMaxVertexCount, double iEdgeLengthConstraintWeight, double iCollisionDistance, double iCollisionWeight, double iBendingResistanceWeight, bool iUseRTree, ref object oMesh)
  {
    if(iReset || system == null)
      system = new MeshGrowthSystem(iStartingMesh);

    system.Grow = iGrow;
    system.MaxVertexCount = iMaxVertexCount;
    system.EdgeLengthConstraintWeight = iEdgeLengthConstraintWeight;
    system.CollisionDistance = iCollisionDistance;
    system.CollisionWeight = iCollisionWeight;
    system.BendingResistanceWeight = iBendingResistanceWeight;
    system.UseRTree = iUseRTree;

    for(int i = 0;i < iSubiterationCount;i++)
      system.Update();

    oMesh = system.GetRhinoMesh();
  }
```
### Use Python to access codes written in C#
在Ghpython Component中输入如下
```python
import clr#导入cluster
clr.AddReferenceToFileAndPath(r"C:\Users\po\AppData\Roaming\Grasshopper\Libraries\MeshGrowth.gha")
#字符串加r，是为了防止字符串被转译。为了告诉编译器这个string是个raw string，例如，\n 在raw string中，是两个字符，\和n， 而不会转意为换行符。

from MeshGrowth import *

if iReset:
    system = MeshGrowthSystem(iStartingMesh)

system.Grow = iGrow
system.MaxVertexCount = iMaxVertexCount
system.UseRTree = iUseRTree
system.EdgeLengthConstraintWeight = iEdgeLengthConstraintWeight
system.CollisionDistance = iCollisionDistance
system.CollisionWeight = iCollisionWeight
system.BendingResistanceWeight = iBendingResistanceWeight

for i in range(iSubiterationCount):
    system.Update()

oMesh = system.GetRhinoMesh()
```