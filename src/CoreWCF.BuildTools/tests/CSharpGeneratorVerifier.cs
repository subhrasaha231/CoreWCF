﻿using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

public static class CSharpGeneratorVerifier<TSourceGenerator>
#if ROSLYN4_0_OR_GREATER
    where TSourceGenerator : IIncrementalGenerator, new()
#else
    where TSourceGenerator : ISourceGenerator, new()
#endif
{
#if ROSLYN4_0_OR_GREATER
    public class Test : CSharpIncrementalGeneratorTest<TSourceGenerator, XUnitVerifier>
#else
    public class Test : CSharpSourceGeneratorTest<TSourceGenerator, XUnitVerifier>
#endif
    {
        public Test()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net50;
            TestState.AdditionalReferences.Add(typeof(System.ServiceModel.ServiceContractAttribute).Assembly);
            TestState.AdditionalReferences.Add(typeof(CoreWCF.ServiceContractAttribute).Assembly);
            TestState.AdditionalReferences.Add(typeof(Microsoft.Extensions.DependencyInjection.IServiceScope).Assembly);
            TestState.AdditionalReferences.Add(typeof(Microsoft.AspNetCore.Http.HttpContext).Assembly);
            TestState.AdditionalReferences.Add(typeof(Microsoft.AspNetCore.Mvc.FromServicesAttribute).Assembly);
            TestState.AdditionalReferences.Add(typeof(Microsoft.AspNetCore.Authorization.IAuthorizeData).Assembly);
        }

        protected override CompilationOptions CreateCompilationOptions()
        {
            var compilationOptions = base.CreateCompilationOptions();
            return compilationOptions.WithSpecificDiagnosticOptions(
                 compilationOptions.SpecificDiagnosticOptions.SetItems(GetNullableWarningsFromCompiler()));
        }

        public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Default;

        private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
        {
            string[] args = { "/warnaserror:nullable" };
            var commandLineArguments = CSharpCommandLineParser.Default.Parse(args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
            var nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

            return nullableWarnings;
        }

        protected override ParseOptions CreateParseOptions()
            => ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);

        protected override bool IsCompilerDiagnosticIncluded(Diagnostic diagnostic, CompilerDiagnostics compilerDiagnostics) => false;
    }
}
