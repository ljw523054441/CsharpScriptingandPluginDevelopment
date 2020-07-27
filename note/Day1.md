## Variables and Data type

```python
a = 12
b = 3.14
myText = "Hello Rhion"
b = 1.23
```

Implicit typing隐式键入

```c#
int a;
a = 12;
double b = 3.14;
string myText = "Hello Rhion";
b = 1.23;
```

Explicit typing显式键入

## Data type

```python
a = 12
a = "I am awesome"
```

Dynamic typing

```c#
int a =12;
string a = "I am awesome"//it`s wrong
```

 Static typing

## The "new" keyword

```c#
Point3d A = new Point3d();//对于非基本数据类型，需要使用new关键字，来声明变量，如果括号中没有参数，默认是0,0,0点
Point3d B = new Point3d(0.0,1.0,2.0);
Point3d C = B;//C将是B的一个独立的分开的副本
```

Point3d中的d表示double，由三个双精度浮点数

## "if" statement

```python
if a>b:
	print("a is bigger than b")
else:
	print("a is smaller than b")
```



```c#
if (a>b)
{
    Print("a is bigger than b");
}
else
{
    Print("a is smaller than b");
}
```



## "for" loop

```python
for i in range(0,5):
	print(i)
```



```c#
for(int i = 0; i < 5; i++)
{
	Print(i.ToSting());
}
```



```c#
for(int i = 0; i < 5; i++)
{
	RhinoApp.WriteLine("Hello World");
}
```



## "for-each" loop

```c#
foreach(Point3d point in points)
{
	//do something with "point">
}
```

## List

```python
myNumber = [0.5,3.14]
myNumbers.append(1.23)
myNumbers.append(2.35)

print(myNumbers[0])
```



```c#
List<double> myNumbers = new List<double>(){0.5,3.14};
myNumbers.Add(1.23);
myNumbers.Add(2.35);

Print(myNumbers[0].ToString());
```



## Example: Subdivision Curve



## Variable and data types in more details

### what is a "variable" anyway?

container

```c#
List<string> a = new List<string>();
a.Add("Hello");
List<string> b = a;
a.Add("Goodbye");
```

先创建一个容器a，a指向了一个空的list

然后空的list中添加了hello这个元素

然后再创建一个容器b，b也指向了这个list

然后再向list中添加了goodbye这个元素

### So, there are two "kinds" of data types

1. value type：当执行复制时，实际值就是被复制，并且最终值的两个独立副本在两个不同的变量内。例如，int,double,Point3d,Vector3D,Circle...  data is copied by the "equal" operator
2. reference type：变量中存储的并不是值本身，而是存储的值的地址，值本身被存储在其他地方。当变量复制时，只是复制了值的引用。所以此时如果修改值本身，这两个变量都是会被修改的。例如，List,Curve,Brep,Surface...  The reference (to the actual data) is copied.

## Functions/Methods

### what is a function?

a way to package a piece of code so that we can conveniently re-use it many times

## Defining a function/method

```python
def ComputeCylinderVolume(r,h):
	baseArea = 3.14 * r * r
	volume = baseArea * h
	return volume
```



```c#
double ComputeCylinderVolume(double r,double h)
{
    double baseArea = 3.14 * r * r;
    double volume = baseArea * h;
    return volume;
}
```

A function/method is a "black box"

## Function Overloading

Same function, different flavours

```c#
double ComputeCylinderVolume1(double r,double h)
{
    return 3.14*r*r*h;
}

double ComputeCylinderVolume2(double r,Point3d bottomCenter, Point3d topCenter){
    double h = bottomCenter.DistanceTo(topCenter);
    return 3.14*r*r*h; 
}
```

Using the same function name for different flavours

```c#
double ComputeCylinderVolume(double r,double h)
{
    return 3.14*r*r*h;
}

double ComputeCylinderVolume(double r,Point3d bottomCenter, Point3d topCenter){
    double h = bottomCenter.DistanceTo(topCenter);
    return 3.14*r*r*h; 
}

double myCylinderVolume = ComputeCylinderVolume(2.13,5.0);

Point3d bottomCenter = Point3d.Origin;
Point3d topCenter = new Pointe3d(2.0,3.0,5.0);
double yourCylinderVolume = ComputeCylinderVolume(2.0,bottomCenter,topCenter);
```

这两个Function的Signature不同

所以下面的不行

```c#
class ScriptInstance : GH_ScriptInstance{
    ...
    double ComputeCylinderVolume(double r,double h){
        ...
    }
    
    void ComputeCylinderVolume(double radius,double height){
        ...
    }
}
```

注意，这两个函数的返回值类型也不同，但是没用，这跟C++中的函数重载是一样的

## What happens to the parameter?

parameter passing: how data are actually input into functions

```c#
void Foo(double someNumber)
{
    someNumber = someNumber + 1.0;
}

//在主函数中
double myNumber = 2.0；
Foo(myNumber);
Print(myNumber.ToString());
//程序输出的结果为2
```

当调用函数并传入myNumber时，myNumber的值将被复制到函数中，所以原来的值并不受影响

## Parameter passing: Pass-by-Value

上面这种机制称为通过值传递，其中值是复制的方式传递给函数，因此在这里看起来好像是我们传递了整个变量

实际上我们并没有传递变量，而是传递的变量的值给函数

## Pass-by-Value for reference type

```c#
void Foo(List<string> texts)
{
    texts.Add("Oh,Yeah!!!");
}

List<string> myTexts = new List<string>();
Print(myTexts.Count.ToString());
Foo(myTexts);
Print(myTexts.Count.ToString());

//输出结果为
//0
//1
```

因为传入的参数是一个reference type,它将指向值本身，因此无论对reference type做任何修改，都会影响原本的值

如果不想做这样的修改，就只能是在进行这样的传递之前用复制，将整个列表复制到另一个列表，必须手动制作一个单独的列表

## Pass-by-Reference: "ref" keyword

```c#
void Foo(double a)
{
    a = a+10;
}

double myNumber = 2.0;
Foo(myNumber);
```

我们知道许多数据类型都是通过引用传递的，这意味着它将影响原始副本

而不通过引用传递的，意味着它将不会影响原始副本

如果在某些情况下，我们想要它影响原始副本

```c#
void Foo(ref double a)//ref关键字
{
    a = a+10;
}

double myNumber = 2.0;
Foo(ref myNumber);//在调用时，也需要使用ref关键字
//程序运行结果为3
```

**Using the ref keyword with the reference-type parameter. Let`s ignore this for now.**

## We can use ref parameters to store result

```c#
void ComputeAreaAndVolume(double r,ref double area,ref doubel volume)
{
    area = 4.0*3.14*r*r;
    volume = 4.0/3.0*3.14*r*r*r
}

double areaResult = 0.0;
doubel volumeResult = 0.0;
ComputeAreaAndVolume(1.32,ref areaResult,ref volumeResult);
```

当函数需要返回多个结果时，可以利用ref关键字，将函数的结果通过参数返回

### Store result in out parameters(better)

```c#
void ComputeAreaAndVolume(double r,out double area,out doubel volume)
{
    area = 4.0*3.14*r*r;
    volume = 4.0/3.0*3.14*r*r*r
}

double areaResult;
doubel volumeResult;
ComputeAreaAndVolume(1.32,out areaResult,out volumeResult);
```

使用out关键字，就可以在调用函数时，只需要声明要输出的变量而不用初始化这些输出变量的实际参数

### An example of  "out" parameter from RhinoCommon

```c#
double t;
boolean success = myCurve.ClosestPoint(myPoint,out t);//找到曲线上相对于目标点最近的一点，t为曲线上的参数

if(success)
    myCurve.PointAt(t);

//为什么会有上面的做法
//因为在查阅ClosestPoint()的语法时
public bool ClosestPoint(
		Point3d testPoint,
    	out double t
)
//这个函数本身返回的是一个bool类型，来告诉你这个函数是否使用成功，但是又需要返回曲线的t值，所谓t选择作为输出参数进行返回
```

## Live example: Adaptively subdividing a curve

## Live example: The random generator object

```c#
private void RunScript(...)
{
    Random myRandomGenerator = new Random();
}
```

## Generate random points within a predefined range

## Persistent Data in the C# Script Component

```c#
public class Script_Instance : GH_ScriptInstance
{
    private void RunScript(object x,object y,ref object A)//主函数
    {
        n += 1;
        Print(n.ToString());
    }

    int n = 0;//主函数之外
    //它在主函数运行结束后并不会消失，而是仍留存在C#电池的内存中
}
```

## Circle Relaxation
