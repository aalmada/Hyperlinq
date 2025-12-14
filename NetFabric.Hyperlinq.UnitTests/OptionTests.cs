using System;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class OptionTests
{
    [Test]
    public void Some_ShouldCreateOptionWithValue()
    {
        var option = Option<int>.Some(42);

        _ = option.HasValue.Must().BeTrue();
        _ = option.Value.Must().BeEqualTo(42);
    }

    [Test]
    public void None_ShouldCreateEmptyOption()
    {
        var option = Option<int>.None();

        _ = option.HasValue.Must().BeFalse();
    }

    [Test]
    public void Value_WhenHasValue_ShouldReturnValue()
    {
        var option = Option<string>.Some("hello");

        _ = option.Value.Must().BeEqualTo("hello");
    }

    [Test]
    public void Value_WhenNoValue_ShouldThrow()
    {
        var option = Option<int>.None();

        Action action = () => { _ = option.Value; };
        _ = action.Must().Throw<InvalidOperationException>();
    }

    [Test]
    public void GetValueOrDefault_WhenHasValue_ShouldReturnValue()
    {
        var option = Option<int>.Some(42);

        _ = option.GetValueOrDefault().Must().BeEqualTo(42);
    }

    [Test]
    public void GetValueOrDefault_WhenNoValue_ShouldReturnDefault()
    {
        var option = Option<int>.None();

        _ = option.GetValueOrDefault().Must().BeEqualTo(0);
    }

    [Test]
    public void GetValueOrDefaultWithValue_WhenHasValue_ShouldReturnValue()
    {
        var option = Option<int>.Some(42);

        _ = option.GetValueOrDefault(99).Must().BeEqualTo(42);
    }

    [Test]
    public void GetValueOrDefaultWithValue_WhenNoValue_ShouldReturnProvidedDefault()
    {
        var option = Option<int>.None();

        _ = option.GetValueOrDefault(99).Must().BeEqualTo(99);
    }

    [Test]
    public void Deconstruct_WhenHasValue_ShouldDeconstructCorrectly()
    {
        var option = Option<int>.Some(42);

        var (hasValue, value) = option;

        _ = hasValue.Must().BeTrue();
        _ = value.Must().BeEqualTo(42);
    }

    [Test]
    public void Deconstruct_WhenNoValue_ShouldDeconstructCorrectly()
    {
        var option = Option<int>.None();

        var (hasValue, value) = option;

        _ = hasValue.Must().BeFalse();
        _ = value.Must().BeEqualTo(0); // default value
    }

    [Test]
    public void Equals_SameValues_ShouldBeEqual()
    {
        var option1 = Option<int>.Some(42);
        var option2 = Option<int>.Some(42);

        _ = option1.Equals(option2).Must().BeTrue();
        _ = (option1 == option2).Must().BeTrue();
        _ = (option1 != option2).Must().BeFalse();
    }

    [Test]
    public void Equals_DifferentValues_ShouldNotBeEqual()
    {
        var option1 = Option<int>.Some(42);
        var option2 = Option<int>.Some(99);

        _ = option1.Equals(option2).Must().BeFalse();
        _ = (option1 == option2).Must().BeFalse();
        _ = (option1 != option2).Must().BeTrue();
    }

    [Test]
    public void Equals_BothNone_ShouldBeEqual()
    {
        var option1 = Option<int>.None();
        var option2 = Option<int>.None();

        _ = option1.Equals(option2).Must().BeTrue();
        _ = (option1 == option2).Must().BeTrue();
    }

    [Test]
    public void Equals_OneNone_ShouldNotBeEqual()
    {
        var option1 = Option<int>.Some(42);
        var option2 = Option<int>.None();

        _ = option1.Equals(option2).Must().BeFalse();
        _ = (option1 == option2).Must().BeFalse();
    }

    [Test]
    public void ToString_WhenHasValue_ShouldShowValue()
    {
        var option = Option<int>.Some(42);

        _ = option.ToString().Must().BeEqualTo("Some(42)");
    }

    [Test]
    public void ToString_WhenNoValue_ShouldShowNone()
    {
        var option = Option<int>.None();

        _ = option.ToString().Must().BeEqualTo("None");
    }

    [Test]
    public void Match_Func_WhenHasValue_ShouldExecuteSome()
    {
        var option = Option<int>.Some(42);

        var result = option.Match(
            some: value => value * 2,
            none: () => 0
        );

        _ = result.Must().BeEqualTo(84);
    }

    [Test]
    public void Match_Func_WhenNoValue_ShouldExecuteNone()
    {
        var option = Option<int>.None();

        var result = option.Match(
            some: value => value * 2,
            none: () => 0
        );

        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void Match_Value_WhenHasValue_ShouldExecuteSome()
    {
        var option = Option<int>.Some(42);

        var result = option.Match(
            some: value => value * 2,
            none: 0
        );

        _ = result.Must().BeEqualTo(84);
    }

    [Test]
    public void Match_Value_WhenNoValue_ShouldReturnNoneValue()
    {
        var option = Option<int>.None();

        var result = option.Match(
            some: value => value * 2,
            none: 10
        );

        _ = result.Must().BeEqualTo(10);
    }

    [Test]
    public void Match_Action_WhenHasValue_ShouldExecuteSome()
    {
        var option = Option<int>.Some(42);
        var result = 0;

        option.Match(
            some: value => result = value,
            none: () => result = -1
        );

        _ = result.Must().BeEqualTo(42);
    }

    [Test]
    public void Match_Action_WhenNoValue_ShouldExecuteNone()
    {
        var option = Option<int>.None();
        var result = 0;

        option.Match(
            some: value => result = value,
            none: () => result = -1
        );

        _ = result.Must().BeEqualTo(-1);
    }

    [Test]
    public void Map_WhenHasValue_ShouldTransformValue()
    {
        var option = Option<int>.Some(42);

        var result = option.Map(value => value.ToString());

        _ = result.HasValue.Must().BeTrue();
        _ = result.Value.Must().BeEqualTo("42");
    }

    [Test]
    public void Map_WhenNoValue_ShouldReturnNone()
    {
        var option = Option<int>.None();

        var result = option.Map(value => value.ToString());

        _ = result.HasValue.Must().BeFalse();
    }

    [Test]
    public void Bind_WhenHasValue_AndBindReturnsSome_ShouldReturnSome()
    {
        var option = Option<int>.Some(42);

        var result = option.Bind(value => Option<string>.Some(value.ToString()));

        _ = result.HasValue.Must().BeTrue();
        _ = result.Value.Must().BeEqualTo("42");
    }

    [Test]
    public void Bind_WhenHasValue_AndBindReturnsNone_ShouldReturnNone()
    {
        var option = Option<int>.Some(42);

        var result = option.Bind(value => Option<string>.None());

        _ = result.HasValue.Must().BeFalse();
    }

    [Test]
    public void Bind_WhenNoValue_ShouldReturnNone()
    {
        var option = Option<int>.None();

        var result = option.Bind(value => Option<string>.Some(value.ToString()));

        _ = result.HasValue.Must().BeFalse();
    }
}
