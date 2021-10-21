using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Generators
{
    [Generator]
    public class DtoGenerator : ISourceGenerator
    {
        //Source generator code
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;

            foreach (var syntaxTree in syntaxTrees)
            {
                //Search for the attribute [Mappable] in the files
                var mappableTypeDeclarations = syntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>()
                    .Where(x => x.AttributeLists.Any(a => a.ToString().StartsWith("[Mappable"))).ToList();

                foreach (var mappableTypeDeclaration in mappableTypeDeclarations)
                {
                    var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                    var usingDirectivesAsText = string.Join("\r\n", usingDirectives);
                    var sourceBuilder = new StringBuilder(usingDirectivesAsText);
                    //Get the class name to generate the DTO object name
                    var className = mappableTypeDeclaration.Identifier.ToString();
                    var genClassName = $"{className}Dto";
                    //Search for properties that must be ignored
                    var ignoreProperties = mappableTypeDeclaration.ChildNodes()
                        .Where(xx => xx is PropertyDeclarationSyntax pdc &&
                        pdc.AttributeLists.Any(xxx => xxx.ToString().StartsWith("[MappableIgnore]")));

                    var newMappableTypeDeclaration = mappableTypeDeclaration.RemoveNodes(ignoreProperties, SyntaxRemoveOptions.KeepEndOfLine);

                    var splitClass = newMappableTypeDeclaration.ToString().Split(new[] { '{' }, 2);
                    //Build the code for the DTO
                    sourceBuilder.Append($@"

namespace GeneratedTypes
{{
    public class {genClassName}
    {{
");
                    sourceBuilder.AppendLine(splitClass[1].Replace(className, genClassName));
                    sourceBuilder.AppendLine("}");
                    //Generate the source text
                    context.AddSource($"Generated_{genClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //To attach a debugger

            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
        }
    }
}
