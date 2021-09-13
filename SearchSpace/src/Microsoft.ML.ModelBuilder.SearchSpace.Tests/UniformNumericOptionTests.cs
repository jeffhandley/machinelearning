﻿// <copyright file="UniformNumericOptionTests.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.ML.ModelBuilder.SearchSpace.Option;
using Xunit;

namespace Microsoft.ML.ModelBuilder.SearchSpace.Tests
{
    public class UniformNumericOptionTests
    {
        [Fact]
        public void Uniform_integer_option_sampling_from_uniform_space_test()
        {
            var option = new UniformIntOption(0, 100);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => i * 0.1);
            var sampleOutputs = sampleInputs.Select(i => option.SampleFromFeatureSpace(new[] { i }));

            sampleOutputs.Select(x => x.AsType<int>()).Should().Equal(0, 10, 20, 30, 40, 50, 60, 70, 80, 90);
        }

        [Fact]
        public void Uniform_log_integer_option_sampling_from_uniform_space_test()
        {
            var option = new UniformIntOption(1, 1024, true);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => i * 0.1);
            var sampleOutputs = sampleInputs.Select(i => option.SampleFromFeatureSpace(new[] { i }));

            sampleOutputs.Select(x => x.AsType<int>()).Should().Equal(1, 2, 4, 8, 16, 32, 64, 128, 256, 511);
        }

        [Fact]
        public void Uniform_integer_option_mapping_to_uniform_space_test()
        {
            var option = new UniformIntOption(0, 100);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => new Parameter(i * 10));
            var sampleOutputs = sampleInputs.Select(i => option.MappingToFeatureSpace(i)[0]);

            sampleOutputs.Should().Equal(0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9);
        }

        [Fact]
        public void Uniform_log_integer_option_mapping_to_uniform_space_test()
        {
            var option = new UniformIntOption(1, 1024, true);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => new Parameter(Convert.ToInt32(Math.Pow(2, i))));
            var sampleOutputs = sampleInputs.Select(i => option.MappingToFeatureSpace(i)[0]).ToArray();

            foreach (var i in Enumerable.Range(0, 10))
            {
                sampleOutputs[i].Should().BeApproximately(0.1 * i, 0.0001);
            }
        }

        [Fact]
        public void Uniform_double_option_sampling_from_uniform_space_test()
        {
            var option = new UniformDoubleOption(0, 100);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => i * 0.1);
            var sampleOutputs = sampleInputs.Select(i => option.SampleFromFeatureSpace(new[] { i }));

            sampleOutputs.Select((x, i) => (x.AsType<double>(), i * 10))
                         .All((x) => Math.Abs(x.Item1 - x.Item2) < 1e-5)
                         .Should().BeTrue();
        }

        [Fact]
        public void Uniform_log_double_option_sampling_from_uniform_space_test()
        {
            var option = new UniformDoubleOption(1, 1024, true);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => i * 0.1);
            var sampleOutputs = sampleInputs.Select(i => option.SampleFromFeatureSpace(new[] { i }));

            sampleOutputs.Select((x, i) => (x.AsType<double>(), Math.Pow(2, i)))
                         .All((x) => Math.Abs(x.Item1 - x.Item2) < 1e-5)
                         .Should().BeTrue();
        }

        [Fact]
        public void Uniform_double_option_mapping_to_uniform_space_test()
        {
            var option = new UniformDoubleOption(0, 100);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => new Parameter(i * 10.0));
            var sampleOutputs = sampleInputs.Select(i => option.MappingToFeatureSpace(i)[0]);

            sampleOutputs.Should().Equal(0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9);
        }

        [Fact]
        public void Uniform_log_double_option_mapping_to_uniform_space_test()
        {
            var option = new UniformDoubleOption(1, 1024, true);

            var sampleInputs = Enumerable.Range(0, 10).Select(i => new Parameter(Math.Pow(2, i)));
            var sampleOutputs = sampleInputs.Select(i => option.MappingToFeatureSpace(i)).ToArray();

            foreach (var i in Enumerable.Range(0, 10))
            {
                sampleOutputs[i][0].Should().BeApproximately(0.1 * i, 1e-5);
            }
        }
    }
}
