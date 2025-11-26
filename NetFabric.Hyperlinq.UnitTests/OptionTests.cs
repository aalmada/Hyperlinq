using System;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class OptionTests
{
    [Test]
    public void Some_ShouldCreateOptionWithValue()
    {
        var option = Option<int>.Some(42);
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(42);
    }
    
    [Test]
    public void None_ShouldCreateEmptyOption()
    {
        var option = Option<int>.None();
        
        option.HasValue.Must().BeFalse();
    }
    
    [Test]
    public void Value_WhenHasValue_ShouldReturnValue()
    {
        var option = Option<string>.Some("hello");
        
        option.Value.Must().BeEqualTo("hello");
    }
    
    [Test]
    public void Value_WhenNoValue_ShouldThrow()
    {
        var option = Option<int>.None();
        
        Action action = () => { var _ = option.Value; };
        action.Must().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void GetValueOrDefault_WhenHasValue_ShouldReturnValue()
    {
        var option = Option<int>.Some(42);
        
        option.GetValueOrDefault().Must().BeEqualTo(42);
    }
    
    [Test]
    public void GetValueOrDefault_WhenNoValue_ShouldReturnDefault()
    {
        var option = Option<int>.None();
        
        option.GetValueOrDefault().Must().BeEqualTo(0);
    }
    
    [Test]
    public void GetValueOrDefaultWithValue_WhenHasValue_ShouldReturnValue()
    {
        var option = Option<int>.Some(42);
        
        option.GetValueOrDefault(99).Must().BeEqualTo(42);
    }
    
    [Test]
    public void GetValueOrDefaultWithValue_WhenNoValue_ShouldReturnProvidedDefault()
    {
        var option = Option<int>.None();
        
        option.GetValueOrDefault(99).Must().BeEqualTo(99);
    }
    
    [Test]
    public void Deconstruct_WhenHasValue_ShouldDeconstructCorrectly()
    {
        var option = Option<int>.Some(42);
        
        var (hasValue, value) = option;
        
        hasValue.Must().BeTrue();
        value.Must().BeEqualTo(42);
    }
    
    [Test]
    public void Deconstruct_WhenNoValue_ShouldDeconstructCorrectly()
    {
        var option = Option<int>.None();
        
        var (hasValue, value) = option;
        
        hasValue.Must().BeFalse();
        value.Must().BeEqualTo(0); // default value
    }
    
    [Test]
    public void Equals_SameValues_ShouldBeEqual()
    {
        var option1 = Option<int>.Some(42);
        var option2 = Option<int>.Some(42);
        
        option1.Equals(option2).Must().BeTrue();
        (option1 == option2).Must().BeTrue();
        (option1 != option2).Must().BeFalse();
    }
    
    [Test]
    public void Equals_DifferentValues_ShouldNotBeEqual()
    {
        var option1 = Option<int>.Some(42);
        var option2 = Option<int>.Some(99);
        
        option1.Equals(option2).Must().BeFalse();
        (option1 == option2).Must().BeFalse();
        (option1 != option2).Must().BeTrue();
    }
    
    [Test]
    public void Equals_BothNone_ShouldBeEqual()
    {
        var option1 = Option<int>.None();
        var option2 = Option<int>.None();
        
        option1.Equals(option2).Must().BeTrue();
        (option1 == option2).Must().BeTrue();
    }
    
    [Test]
    public void Equals_OneNone_ShouldNotBeEqual()
    {
        var option1 = Option<int>.Some(42);
        var option2 = Option<int>.None();
        
        option1.Equals(option2).Must().BeFalse();
        (option1 == option2).Must().BeFalse();
    }
    
    [Test]
    public void ToString_WhenHasValue_ShouldShowValue()
    {
        var option = Option<int>.Some(42);
        
        option.ToString().Must().BeEqualTo("Some(42)");
    }
    
    [Test]
    public void ToString_WhenNoValue_ShouldShowNone()
    {
        var option = Option<int>.None();
        
        option.ToString().Must().BeEqualTo("None");
    }
}
