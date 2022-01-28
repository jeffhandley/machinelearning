# Model builder search space

## Introduction
This library contains all search-space related code, including creating, updating and sampling. 

![image](https://user-images.githubusercontent.com/16876986/133147775-4fc4ba54-255e-41cf-9079-dc523706893b.png)

### Numeric option | Choice option

Option is the most basic unit in MB Search Space. And it represent a searching range for a specific hyper parameter. An Numeric option is an option where its corresponding parameter is of numeric type and usually its searching space is a certain range between min and max value. A choice option is an option where its parameter only accept specific values, like boolean (True/False), enum, etc..

In implementation, each option represents a mapper function which maps its value into a known space. Thereby free tuners from managing the details of different options.

### Search space | Nested search space

Search space is a group of options and it's no special than a key-value dictionary where key is option name and value is option itself. A search space is also an option because it also maps its value into a known space through options. And that's how nested search space being realized.

### Attribute API | CRUD API

API for creating search space. The Attribution API allow users to mark a property as an option, which makes creating search space easier. CURD API allows user to create search space from draft and edit it like editing a dictionary.

### Algo API
Provides some basic tuning algoritions (GridSearch, etc..)

## Examples

``` csharp
private class BasicSearchSpace
{
    [Range(-1000, 1000, init: 0)]
    public int UniformInt { get; set; }

    [Choice("a", "b", "c", "d")]
    public string ChoiceStr { get; set; }
    
    [Option]
    public BasicSearchSpace NestSS { get; set; }
}

// Which is equal to the folloing CRUD API
/*
var ss = new SearchSpace()
{
  {"UniformInt", new UniformIntOption(-1000, 1000, init: 0)},
  {"ChoiceStr", new Choice("a", "b", "c", "d")},
  {"NestSS", new BasicSearchSpace()},
};
*/

// Create search space
var ss = new SearchSpace<BasicSearchSpace>();

// Create a tuner which always return default value
// it's defined in https://github.com/dotnet/machinelearning-tools/blob/928f2c1de51cae1c0df4c58db6d36c72ece6fa7c/src/Microsoft.ML.ModelBuilder.SearchSpace/Tuner/DefaultValueTuner.cs
var tuner = new DefaultValueTuner(ss);

// Get one sampling result from tuner
BasicSearchSpace param = tuner.Propose();
// param.UniformInt.Should().Be(0);
// param.ChoiceStr.Should().Be("a"); // no default value designate for ChoiceStr, so the first value will be used.
// param.NestSS.UniformInt.Should().Be(0);

// Or you can also sample from scratch
// The feature of ss is a 4-d [0, 1) vector.
BasicSearchSpace param2 = ss.SampleFromFeatureSpace(new[] {0.5, 0, 0.5, 0});
// (param == param2).Should().BeTrue();

// You can also map param back to feature space
ss.MappingToFeatureSpace(param).Should().BeEquivalentTo(0.5, 0, 0.5, 0);
```

